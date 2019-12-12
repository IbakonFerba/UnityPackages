using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A Visual Element that can render a tiled image with a set offset, tiling and scale</para>
    ///
    /// v1.2 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class TiledImage : ImmediateModeElement
    {
        // ######################## STRUCTS & CLASSES ######################## //
        public new class UxmlFactory : UxmlFactory<TiledImage>
        {
        }


        // ######################## PROPERTIES ######################## //
        private Material ImageMaterial
        {
            get
            {
                if (!_imageMaterial)
                {
                    Shader shader = Shader.Find("Hidden/Internal-GUIRoundedRect");
                    _imageMaterial = new Material(shader);
                    _imageMaterial.hideFlags = HideFlags.HideAndDontSave;
                    _imageMaterial.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                    _imageMaterial.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _imageMaterial.SetInt("_Cull", (int) UnityEngine.Rendering.CullMode.Off);
                    _imageMaterial.SetInt("_ZWrite", 0);
                }
#if UNITY_2019_3_OR_NEWER
                _imageMaterial.SetInt("_ManualTex2SRGB", 0);
#else
                _imageMaterial.SetInt("_ManualTex2SRGB", 1);
#endif

                return _imageMaterial;
            }
        }

        public float BorderTopLeftRadius
        {
            get => _borderRadii[0];
            set
            {
                _borderRadii[0] = value;
                _borderStyleOverride = true;
                ImageMaterial.SetFloatArray("_CornerRadiuses", _borderRadii);
            }
        }

        public float BorderTopRightRadius
        {
            get => _borderRadii[1];
            set
            {
                _borderRadii[1] = value;
                _borderStyleOverride = true;
                ImageMaterial.SetFloatArray("_CornerRadiuses", _borderRadii);
            }
        }

        public float BorderBottomRightRadius
        {
            get => _borderRadii[2];
            set
            {
                _borderRadii[2] = value;
                _borderStyleOverride = true;
                ImageMaterial.SetFloatArray("_CornerRadiuses", _borderRadii);
            }
        }

        public float BorderBottomLeftRadius
        {
            get => _borderRadii[3];
            set
            {
                _borderRadii[3] = value;
                _borderStyleOverride = true;
                ImageMaterial.SetFloatArray("_CornerRadiuses", _borderRadii);
            }
        }


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        #region USS_PROPERTIES

        private static readonly CustomStyleProperty<Texture2D> _image_property = new CustomStyleProperty<Texture2D>("--tiled-image");
        private static readonly CustomStyleProperty<float> _tiling_property = new CustomStyleProperty<float>("--tiling");

        #endregion

        private Material _imageMaterial;

        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _tiling = new Vector2(1, 1);

        private float[] _borderRadii = new float[4];
        private bool _borderStyleOverride = false;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public TiledImage()
        {
            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        }

        public TiledImage(Texture2D texture)
        {
            SetTexture(texture);
            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        }

        #endregion

        #region FACTORIES

        /// <summary>
        /// Returns a new tiled image that fills the entire space it has available
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static TiledImage CreateBackground(Texture2D texture)
        {
            TiledImage background = new TiledImage();
            background.SetTexture(texture);
            background.AddToClassList(UssClasses.FULL);

            return background;
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            ICustomStyle styles = evt.customStyle;

            if (styles.TryGetValue(_image_property, out Texture2D value))
                SetTexture(value);
            if (styles.TryGetValue(_tiling_property, out float tilingValue))
                SetTiling(tilingValue);
        }

        protected override void ImmediateRepaint()
        {
            ImageMaterial.SetFloatArray("_Rect", new[] {localBound.x, localBound.y, localBound.width, localBound.height});
            if (!_borderStyleOverride)
                ImageMaterial.SetFloatArray("_CornerRadiuses",
                    new[] {resolvedStyle.borderTopLeftRadius, resolvedStyle.borderTopRightRadius, resolvedStyle.borderBottomRightRadius, resolvedStyle.borderBottomLeftRadius});
            ImageMaterial.SetFloatArray("_BorderWidths", new[] {layout.width, layout.height, layout.width, layout.height});

            ImageMaterial.SetPass(0);

            // calculate scaling values so the image is not distorted no matter what the aspect ratio of the element is
            float xScale = layout.width * _texture.texelSize.x;
            float yScale = layout.height * _texture.texelSize.y;

            // draw a textured quad
            GL.Begin(GL.QUADS);
            GL.TexCoord(new Vector3(_position.x + 0 * xScale, _position.y - 0 * yScale, 0));
            GL.Vertex(new Vector3(layout.xMin, layout.yMin, 0));

            GL.TexCoord(new Vector3(_position.x + 1 * xScale, _position.y - 0 * yScale, 0));
            GL.Vertex(new Vector3(layout.xMax, layout.yMin, 0));

            GL.TexCoord(new Vector3(_position.x + 1 * xScale, _position.y - 1 * yScale, 0));
            GL.Vertex(new Vector3(layout.xMax, layout.yMax, 0));

            GL.TexCoord(new Vector3(_position.x + 0 * xScale, _position.y - 1 * yScale, 0));
            GL.Vertex(new Vector3(layout.xMin, layout.yMax, 0));
            GL.End();
        }

        #endregion


        // ######################## SETTER ######################## //

        #region SETTER

        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
            ImageMaterial.mainTexture = texture;
        }

        public void SetOffset(Vector2 offset)
        {
            float xScale = worldBound.width * _texture.texelSize.x;
            float yScale = worldBound.height * _texture.texelSize.y;
            _position = new Vector2((-offset.x / worldBound.width) * xScale, (offset.y / worldBound.height) * yScale);
            MarkDirtyRepaint();
        }

        public void SetScale(Vector2 scale)
        {
            ImageMaterial.mainTextureScale = scale * _tiling;
            MarkDirtyRepaint();
        }

        public void SetTiling(float tiling)
        {
            _tiling.x = tiling;
            _tiling.y = tiling;

            ImageMaterial.mainTextureScale *= _tiling;
        }

        #endregion
    }
}