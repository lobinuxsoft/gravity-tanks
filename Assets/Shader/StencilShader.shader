Shader "Custom/StencilShader"
{
    Properties
    {
        _MaskRef ("Mask reference number", Int) = 1
    }

    SubShader
    {
        Pass
        {
            ColorMask 0
            Zwrite Off

            Stencil
            {
                Ref [_MaskRef]
                Comp Always
                Pass Replace
            }
        }
    }
}