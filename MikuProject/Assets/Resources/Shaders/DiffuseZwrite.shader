Shader "Custom/Transparent Diffuse ZWrite" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 200

    // デプスバッファのみにレンダリングする追加のパス
    Pass {
        ZWrite On
        ColorMask 0
    }

    // forward renderingパスをTransparent/Diffuseから渡します
    UsePass "Transparent/Diffuse/FORWARD"
}
Fallback "Transparent/VertexLit"
}