Shader "UIGrey"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _AlphaClip("_AlphaClip", Float) = 0
        _GreyColor("_GreyColor", Vector) = (0, 1, 0, 0)
        _GreyLerp("_GreyLerp", Float) = 0
        [HideInInspector]_EmissionColor("Color", Color) = (1, 1, 1, 1)
        [HideInInspector]_RenderQueueType("Float", Float) = 1
        [HideInInspector][ToggleUI]_AddPrecomputedVelocity("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_DepthOffsetEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_ConservativeDepthOffsetEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_TransparentWritingMotionVec("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_AlphaCutoffEnable("Boolean", Float) = 1
        [HideInInspector]_TransparentSortPriority("_TransparentSortPriority", Float) = 0
        [HideInInspector][ToggleUI]_UseShadowThreshold("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_DoubleSidedEnable("Boolean", Float) = 0
        [HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)]_DoubleSidedNormalMode("Float", Float) = 2
        [HideInInspector]_DoubleSidedConstants("Vector4", Vector) = (1, 1, -1, 0)
        [HideInInspector][Enum(Auto, 0, On, 1, Off, 2)]_DoubleSidedGIMode("Float", Float) = 0
        [HideInInspector][ToggleUI]_TransparentDepthPrepassEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_TransparentDepthPostpassEnable("Boolean", Float) = 0
        [HideInInspector]_SurfaceType("Float", Float) = 0
        [HideInInspector]_BlendMode("Float", Float) = 0
        [HideInInspector]_SrcBlend("Float", Float) = 1
        [HideInInspector]_DstBlend("Float", Float) = 0
        [HideInInspector]_AlphaSrcBlend("Float", Float) = 1
        [HideInInspector]_AlphaDstBlend("Float", Float) = 0
        [HideInInspector][ToggleUI]_AlphaToMask("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_AlphaToMaskInspectorValue("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("Boolean", Float) = 1
        [HideInInspector][ToggleUI]_TransparentZWrite("Boolean", Float) = 0
        [HideInInspector]_CullMode("Float", Float) = 2
        [HideInInspector][ToggleUI]_EnableFogOnTransparent("Boolean", Float) = 1
        [HideInInspector]_CullModeForward("Float", Float) = 2
        [HideInInspector][Enum(Front, 1, Back, 2)]_TransparentCullMode("Float", Float) = 2
        [HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)]_OpaqueCullMode("Float", Float) = 2
        [HideInInspector]_ZTestDepthEqualForOpaque("Float", Int) = 4
        [HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)]_ZTestTransparent("Float", Float) = 4
        [HideInInspector][ToggleUI]_TransparentBackfaceEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_EnableBlendModePreserveSpecularLighting("Boolean", Float) = 0
        [HideInInspector]_StencilRef("Float", Int) = 0
        [HideInInspector]_StencilWriteMask("Float", Int) = 6
        [HideInInspector]_StencilRefDepth("Float", Int) = 0
        [HideInInspector]_StencilWriteMaskDepth("Float", Int) = 8
        [HideInInspector]_StencilRefMV("Float", Int) = 32
        [HideInInspector]_StencilWriteMaskMV("Float", Int) = 40
        [HideInInspector]_StencilRefDistortionVec("Float", Int) = 4
        [HideInInspector]_StencilWriteMaskDistortionVec("Float", Int) = 4
        [HideInInspector]_StencilWriteMaskGBuffer("Float", Int) = 14
        [HideInInspector]_StencilRefGBuffer("Float", Int) = 2
        [HideInInspector]_ZTestGBuffer("Float", Int) = 4
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask2("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "HDRenderPipeline"
            "RenderType" = "HDUnlitShader"
            "Queue" = "AlphaTest+0"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = "HDUnlitSubTarget"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

        // Render State
        Cull[_CullMode]
    ZWrite On
    ColorMask 0
    ZClip[_ZClip]

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma instancing_options renderinglayer
    #pragma target 4.5
    #pragma vertex Vert
    #pragma fragment Frag
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
    #pragma multi_compile_instancing

        // Keywords
        #pragma shader_feature_local _ _ALPHATEST_ON
    #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
    #pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
    #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
    #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
    #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
        // GraphKeywords: <None>

        // Early Instancing Defines
        // DotsInstancingOptions: <None>

        // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
        // HybridV1InjectedBuiltinProperties: <None>

        // For custom interpolators to inject a substruct definition before FragInputs definition,
        // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
        struct CustomInterpolators
    {
    };
    #define USE_CUSTOMINTERP_SUBSTRUCT



    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
        /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
    };
    #endif

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

    // --------------------------------------------------
    // Defines

    // Attribute
    #define ATTRIBUTES_NEED_NORMAL
    #define ATTRIBUTES_NEED_TANGENT
    #define ATTRIBUTES_NEED_TEXCOORD0
    #define VARYINGS_NEED_TEXCOORD0

    #define HAVE_MESH_MODIFICATION



    #define SHADERPASS SHADERPASS_SHADOWS


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 interp0 : INTERP0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.texCoord0 = input.interp0.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions
// GraphFunctions: <None>

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit


    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "META"
    Tags
    {
        "LightMode" = "META"
    }

    // Render State
    Cull Off

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma instancing_options renderinglayer
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature _ EDITOR_VISUALIZATION
#pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TEXCOORD0
#define ATTRIBUTES_NEED_TEXCOORD1
#define ATTRIBUTES_NEED_TEXCOORD2
#define ATTRIBUTES_NEED_TEXCOORD3
#define VARYINGS_NEED_POSITION_WS
#define VARYINGS_NEED_POSITIONPREDISPLACEMENT_WS
#define VARYINGS_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD1
#define VARYINGS_NEED_TEXCOORD2
#define VARYINGS_NEED_TEXCOORD3

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
#define RAYTRACING_SHADER_GRAPH_DEFAULT


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 uv0 : TEXCOORD0;
     float4 uv1 : TEXCOORD1;
     float4 uv2 : TEXCOORD2;
     float4 uv3 : TEXCOORD3;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 positionRWS;
     float3 positionPredisplacementRWS;
     float4 texCoord0;
     float4 texCoord1;
     float4 texCoord2;
     float4 texCoord3;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 interp0 : INTERP0;
     float3 interp1 : INTERP1;
     float4 interp2 : INTERP2;
     float4 interp3 : INTERP3;
     float4 interp4 : INTERP4;
     float4 interp5 : INTERP5;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.positionRWS;
    output.interp1.xyz = input.positionPredisplacementRWS;
    output.interp2.xyzw = input.texCoord0;
    output.interp3.xyzw = input.texCoord1;
    output.interp4.xyzw = input.texCoord2;
    output.interp5.xyzw = input.texCoord3;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.positionRWS = input.interp0.xyz;
    output.positionPredisplacementRWS = input.interp1.xyz;
    output.texCoord0 = input.interp2.xyzw;
    output.texCoord1 = input.interp3.xyzw;
    output.texCoord2 = input.interp4.xyzw;
    output.texCoord3 = input.interp5.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);


    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorVertMeshCustomInterpolation' */

    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.positionRWS = input.positionRWS;
    output.positionPredisplacementRWS = input.positionPredisplacementRWS;
    output.texCoord0 = input.texCoord0;
    output.texCoord1 = input.texCoord1;
    output.texCoord2 = input.texCoord2;
    output.texCoord3 = input.texCoord3;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorVaryingsToFragInputs' */

    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassLightTransport.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "ScenePickingPass"
    Tags
    {
        "LightMode" = "Picking"
    }

    // Render State
    Cull[_CullMode]

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma instancing_options renderinglayer
#pragma editor_sync_compilation
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TANGENT_TO_WORLD
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_DEPTH_ONLY
#define SCENEPICKINGPASS 1


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 normalWS;
     float4 tangentWS;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 interp0 : INTERP0;
     float4 interp1 : INTERP1;
     float4 interp2 : INTERP2;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.normalWS;
    output.interp1.xyzw = input.tangentWS;
    output.interp2.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.normalWS = input.interp0.xyz;
    output.tangentWS = input.interp1.xyzw;
    output.texCoord0 = input.interp2.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "SceneSelectionPass"
    Tags
    {
        "LightMode" = "SceneSelectionPass"
    }

    // Render State
    Cull Off

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma instancing_options renderinglayer
#pragma editor_sync_compilation
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_DEPTH_ONLY
#define RAYTRACING_SHADER_GRAPH_DEFAULT
#define SCENESELECTIONPASS 1


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 interp0 : INTERP0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.texCoord0 = input.interp0.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "MotionVectors"
    Tags
    {
        "LightMode" = "MotionVectors"
    }

    // Render State
    Cull[_CullMode]
ZWrite On
Stencil
{
WriteMask[_StencilWriteMaskMV]
Ref[_StencilRefMV]
CompFront Always
PassFront Replace
CompBack Always
PassBack Replace
}
AlphaToMask[_AlphaToMask]

// Debug
// <None>

// --------------------------------------------------
// Pass

HLSLPROGRAM

// Pragmas
#pragma instancing_options renderinglayer
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature_local _ _ALPHATOMASK_ON
#pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_POSITION_WS
#define VARYINGS_NEED_TANGENT_TO_WORLD
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_MOTION_VECTORS


// Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
// The ShaderGraph don't support correctly migration of this node as it serialize all the node data
// in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
// to still allow us to rename the field and keyword of this node without breaking existing code.
#ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
#define RAYTRACING_SHADER_GRAPH_HIGH
#endif

#ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
#define RAYTRACING_SHADER_GRAPH_LOW
#endif
// end

#ifndef SHADER_UNLIT
// We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
// VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
    #define VARYINGS_NEED_CULLFACE
#endif
#endif

// Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 positionRWS;
     float3 normalWS;
     float4 tangentWS;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 interp0 : INTERP0;
     float3 interp1 : INTERP1;
     float4 interp2 : INTERP2;
     float4 interp3 : INTERP3;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.positionRWS;
    output.interp1.xyz = input.normalWS;
    output.interp2.xyzw = input.tangentWS;
    output.interp3.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.positionRWS = input.interp0.xyz;
    output.normalWS = input.interp1.xyz;
    output.tangentWS = input.interp2.xyzw;
    output.texCoord0 = input.interp3.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.positionRWS = input.positionRWS;
    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassMotionVectors.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "DepthForwardOnly"
    Tags
    {
        "LightMode" = "DepthForwardOnly"
    }

    // Render State
    Cull[_CullMode]
ZWrite On
Stencil
{
WriteMask[_StencilWriteMaskDepth]
Ref[_StencilRefDepth]
CompFront Always
PassFront Replace
CompBack Always
PassBack Replace
}
AlphaToMask[_AlphaToMask]

// Debug
// <None>

// --------------------------------------------------
// Pass

HLSLPROGRAM

// Pragmas
#pragma instancing_options renderinglayer
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature_local _ _ALPHATOMASK_ON
#pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TANGENT_TO_WORLD
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_DEPTH_ONLY


// Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
// The ShaderGraph don't support correctly migration of this node as it serialize all the node data
// in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
// to still allow us to rename the field and keyword of this node without breaking existing code.
#ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
#define RAYTRACING_SHADER_GRAPH_HIGH
#endif

#ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
#define RAYTRACING_SHADER_GRAPH_LOW
#endif
// end

#ifndef SHADER_UNLIT
// We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
// VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
    #define VARYINGS_NEED_CULLFACE
#endif
#endif

// Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 normalWS;
     float4 tangentWS;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 interp0 : INTERP0;
     float4 interp1 : INTERP1;
     float4 interp2 : INTERP2;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.normalWS;
    output.interp1.xyzw = input.tangentWS;
    output.interp2.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.normalWS = input.interp0.xyz;
    output.tangentWS = input.interp1.xyzw;
    output.texCoord0 = input.interp2.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "ForwardOnly"
    Tags
    {
        "LightMode" = "ForwardOnly"
    }

    // Render State
    Cull[_CullModeForward]
Blend[_SrcBlend][_DstBlend],[_AlphaSrcBlend][_AlphaDstBlend]
ZTest[_ZTestDepthEqualForOpaque]
ZWrite[_ZWrite]
ColorMask[_ColorMaskTransparentVelOne] 1
ColorMask[_ColorMaskTransparentVelTwo] 2
Stencil
{
WriteMask[_StencilWriteMask]
Ref[_StencilRef]
CompFront Always
PassFront Replace
CompBack Always
PassBack Replace
}

// Debug
// <None>

// --------------------------------------------------
// Pass

HLSLPROGRAM

// Pragmas
#pragma instancing_options renderinglayer
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
#pragma multi_compile _ DEBUG_DISPLAY
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_POSITION_WS
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_FORWARD_UNLIT
#define RAYTRACING_SHADER_GRAPH_DEFAULT


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 positionRWS;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float3 interp0 : INTERP0;
     float4 interp1 : INTERP1;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.positionRWS;
    output.interp1.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.positionRWS = input.interp0.xyz;
    output.texCoord0 = input.interp1.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
    float4 VTPackedFeedback;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    {
        surface.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
    }
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.positionRWS = input.positionRWS;
    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForwardUnlit.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "FullScreenDebug"
    Tags
    {
        "LightMode" = "FullScreenDebug"
    }

    // Render State
    Cull[_CullMode]
ZTest LEqual
ZWrite Off

// Debug
// <None>

// --------------------------------------------------
// Pass

HLSLPROGRAM

// Pragmas
#pragma instancing_options renderinglayer
#pragma target 4.5
#pragma vertex Vert
#pragma fragment Frag
#pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
#pragma multi_compile_instancing

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    struct CustomInterpolators
{
};
#define USE_CUSTOMINTERP_SUBSTRUCT



// TODO: Merge FragInputsVFX substruct with CustomInterpolators.
#ifdef HAVE_VFX_MODIFICATION
struct FragInputsVFX
{
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0

#define HAVE_MESH_MODIFICATION



#define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG
#define RAYTRACING_SHADER_GRAPH_DEFAULT


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct AttributesMesh
{
     float3 positionOS : POSITION;
     float3 normalOS : NORMAL;
     float4 tangentOS : TANGENT;
     float4 uv0 : TEXCOORD0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : INSTANCEID_SEMANTIC;
    #endif
};
struct VaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};
struct VertexDescriptionInputs
{
     float3 ObjectSpaceNormal;
     float3 ObjectSpaceTangent;
     float3 ObjectSpacePosition;
};
struct SurfaceDescriptionInputs
{
     float4 uv0;
};
struct PackedVaryingsMeshToPS
{
    SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
     float4 interp0 : INTERP0;
    #if UNITY_ANY_INSTANCING_ENABLED
     uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
};

    PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
{
    PackedVaryingsMeshToPS output;
    ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
    output.positionCS = input.positionCS;
    output.interp0.xyzw = input.texCoord0;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}

VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
{
    VaryingsMeshToPS output;
    output.positionCS = input.positionCS;
    output.texCoord0 = input.interp0.xyzw;
    #if UNITY_ANY_INSTANCING_ENABLED
    output.instanceID = input.instanceID;
    #endif
    return output;
}


// --------------------------------------------------
// Graph


// Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif

VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
#ifdef USE_CUSTOMINTERP_SUBSTRUCT
    #ifdef TESSELLATION_ON
    , inout VaryingsMeshToDS varyings
    #else
    , inout VaryingsMeshToPS varyings
    #endif
#endif
#ifdef HAVE_VFX_MODIFICATION
        , AttributesElement element
#endif
    )
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    // Override time parameters with used one (This is required to correctly handle motion vector for vertex animation based on time)

    // evaluate vertex graph
#ifdef HAVE_VFX_MODIFICATION
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    // Fetch the vertex graph properties for the particle instance.
    GetElementVertexProperties(element, properties);

    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
#else
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
#endif

    // copy graph output to the results
    input.positionOS = vertexDescription.Position;
    input.normalOS = vertexDescription.Normal;
    input.tangentOS.xyz = vertexDescription.Tangent;



    return input;
}

#if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
// Return precomputed Velocity in object space
float3 GetCustomVelocity(AttributesMesh input)
{
    // build graph inputs
    VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
    VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
    return vertexDescription.CustomVelocity;
}
#endif

FragInputs BuildFragInputs(VaryingsMeshToPS input)
{
    FragInputs output;
    ZERO_INITIALIZE(FragInputs, output);

    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
    // to compute normals which are then passed on elsewhere to compute other values...
    output.tangentToWorld = k_identity3x3;
    output.positionSS = input.positionCS;       // input.positionCS is SV_Position

    output.texCoord0 = input.texCoord0;

#ifdef HAVE_VFX_MODIFICATION
    // FragInputs from VFX come from two places: Interpolator or CBuffer.
    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

#endif

    // splice point to copy custom interpolator fields from varyings to frag inputs


    return output;
}

// existing HDRP code uses the combined function to go directly from packed to frag inputs
FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
    return BuildFragInputs(unpacked);
}
    SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI


    return output;
}

    // --------------------------------------------------
    // Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassFullScreenDebug.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
    }
        SubShader
{
    Tags
    {
        "RenderPipeline" = "HDRenderPipeline"
        "RenderType" = "HDUnlitShader"
        "Queue" = "AlphaTest+0"
        "ShaderGraphShader" = "true"
        "ShaderGraphTargetId" = "HDUnlitSubTarget"
    }
            
         Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask2]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

    Pass
    {
        Name "IndirectDXR"
        Tags
        {
            "LightMode" = "IndirectDXR"
        }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma target 5.0
#pragma raytracing surface_shader
#pragma only_renderers d3d11 ps5

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
#pragma multi_compile _ DEBUG_DISPLAY
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0




#define SHADERPASS SHADERPASS_RAYTRACING_INDIRECT
#define SHADOW_LOW
#define RAYTRACING_SHADER_GRAPH_RAYTRACED


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct SurfaceDescriptionInputs
{
     float4 uv0;
};

    //Interpolator Packs: <None>

    // --------------------------------------------------
    // Graph


    // Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
// GraphVertex: <None>

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif
SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

// --------------------------------------------------
// Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingIndirect.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "VisibilityDXR"
    Tags
    {
        "LightMode" = "VisibilityDXR"
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma target 5.0
#pragma raytracing surface_shader
#pragma only_renderers d3d11 ps5

    // Keywords
    #pragma multi_compile _ TRANSPARENT_COLOR_SHADOW
#pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0




#define SHADERPASS SHADERPASS_RAYTRACING_VISIBILITY


// Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
// The ShaderGraph don't support correctly migration of this node as it serialize all the node data
// in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
// to still allow us to rename the field and keyword of this node without breaking existing code.
#ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
#define RAYTRACING_SHADER_GRAPH_HIGH
#endif

#ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
#define RAYTRACING_SHADER_GRAPH_LOW
#endif
// end

#ifndef SHADER_UNLIT
// We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
// VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
    #define VARYINGS_NEED_CULLFACE
#endif
#endif

// Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct SurfaceDescriptionInputs
{
     float4 uv0;
};

    //Interpolator Packs: <None>

    // --------------------------------------------------
    // Graph


    // Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
// GraphVertex: <None>

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif
SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

// --------------------------------------------------
// Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingVisibility.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "ForwardDXR"
    Tags
    {
        "LightMode" = "ForwardDXR"
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma target 5.0
#pragma raytracing surface_shader
#pragma only_renderers d3d11 ps5

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
#pragma multi_compile _ DEBUG_DISPLAY
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0




#define SHADERPASS SHADERPASS_RAYTRACING_FORWARD
#define SHADOW_LOW
#define RAYTRACING_SHADER_GRAPH_RAYTRACED


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct SurfaceDescriptionInputs
{
     float4 uv0;
};

    //Interpolator Packs: <None>

    // --------------------------------------------------
    // Graph


    // Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
// GraphVertex: <None>

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif
SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

// --------------------------------------------------
// Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingForward.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "GBufferDXR"
    Tags
    {
        "LightMode" = "GBufferDXR"
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma target 5.0
#pragma raytracing surface_shader
#pragma only_renderers d3d11 ps5

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
#pragma multi_compile _ DEBUG_DISPLAY
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0




#define SHADERPASS SHADERPASS_RAYTRACING_GBUFFER
#define SHADOW_LOW
#define RAYTRACING_SHADER_GRAPH_RAYTRACED


    // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
    // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
    // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
    // to still allow us to rename the field and keyword of this node without breaking existing code.
    #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
    #define RAYTRACING_SHADER_GRAPH_HIGH
    #endif

    #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
    #define RAYTRACING_SHADER_GRAPH_LOW
    #endif
    // end

    #ifndef SHADER_UNLIT
    // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
    // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
    #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
        #define VARYINGS_NEED_CULLFACE
    #endif
    #endif

    // Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/Deferred/RaytracingIntersectonGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/StandardLit/StandardLit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct SurfaceDescriptionInputs
{
     float4 uv0;
};

    //Interpolator Packs: <None>

    // --------------------------------------------------
    // Graph


    // Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
// GraphVertex: <None>

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif
SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

// --------------------------------------------------
// Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingGBuffer.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
Pass
{
    Name "PathTracingDXR"
    Tags
    {
        "LightMode" = "PathTracingDXR"
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma target 5.0
#pragma raytracing surface_shader
#pragma only_renderers d3d11 ps5

    // Keywords
    #pragma shader_feature_local _ _ALPHATEST_ON
#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
#pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
    // GraphKeywords: <None>

    // Early Instancing Defines
    // DotsInstancingOptions: <None>

    // Injected Instanced Properties (must be included before UnityInstancing.hlsl)
    // HybridV1InjectedBuiltinProperties: <None>

    // For custom interpolators to inject a substruct definition before FragInputs definition,
    // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */


    // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
    #ifdef HAVE_VFX_MODIFICATION
    struct FragInputsVFX
    {
    /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
};
#endif

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition

// --------------------------------------------------
// Defines

// Attribute
#define ATTRIBUTES_NEED_TEXCOORD0
#define VARYINGS_NEED_TEXCOORD0




#define SHADERPASS SHADERPASS_PATH_TRACING


// Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
// The ShaderGraph don't support correctly migration of this node as it serialize all the node data
// in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
// to still allow us to rename the field and keyword of this node without breaking existing code.
#ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
#define RAYTRACING_SHADER_GRAPH_HIGH
#endif

#ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
#define RAYTRACING_SHADER_GRAPH_LOW
#endif
// end

#ifndef SHADER_UNLIT
// We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
// VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
    #define VARYINGS_NEED_CULLFACE
#endif
#endif

// Specific Material Define
// Setup a define to say we are an unlit shader
#define SHADER_UNLIT

// Following Macro are only used by Unlit material
#if defined(_ENABLE_SHADOW_MATTE) && (SHADERPASS == SHADERPASS_FORWARD_UNLIT || SHADERPASS == SHADERPASS_PATH_TRACING)
#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
#define HAS_LIGHTLOOP
#endif
    // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it

    // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
    // we should have a code like this:
    // if !defined(_DISABLE_SSR_TRANSPARENT)
    // pragma multi_compile _ WRITE_NORMAL_BUFFER
    // endif
    // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
    // it based on if SSR transparent in frame settings and not (and stripper can strip it).
    // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
    // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
    // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
    #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
        #define WRITE_NORMAL_BUFFER
    #endif
    #endif

    #ifndef DEBUG_DISPLAY
        // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
        // Don't do it with debug display mode as it is possible there is no depth prepass in this case
        #if !defined(_SURFACE_TYPE_TRANSPARENT)
            #if SHADERPASS == SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
            #elif SHADERPASS == SHADERPASS_GBUFFER
            #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
            #endif
        #endif
    #endif

    // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
    #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
        #define _DEFERRED_CAPABLE_MATERIAL
    #endif

    // Translate transparent motion vector define
    #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
        #define _WRITE_TRANSPARENT_MOTION_VECTOR
    #endif

    // -- Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _AlphaClip;
float4 _GreyColor;
float _GreyLerp;
float4 _EmissionColor;
float _UseShadowThreshold;
float4 _DoubleSidedConstants;
float _BlendMode;
float _EnableBlendModePreserveSpecularLighting;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Includes
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
    // GraphIncludes: <None>

    // --------------------------------------------------
    // Structs and Packing

    struct SurfaceDescriptionInputs
{
     float4 uv0;
};

    //Interpolator Packs: <None>

    // --------------------------------------------------
    // Graph


    // Graph Functions

void Unity_DotProduct_float4(float4 A, float4 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

// Graph Vertex
// GraphVertex: <None>

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float3 Emission;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.tex, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.samplerstate, _Property_0a79c9d8f1d847ffb06c0e0c56477966_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_R_4 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.r;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_G_5 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.g;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_B_6 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.b;
    float _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7 = _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0.a;
    float4 _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0 = _GreyColor;
    float _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2;
    Unity_DotProduct_float4(_SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, _Property_d2925c5718a145c59e8307cafb8d0cfe_Out_0, _DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2);
    float _Property_2dc11350183641b1976e0cb3796b76ff_Out_0 = _GreyLerp;
    float4 _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3;
    Unity_Lerp_float4((_DotProduct_1b88b1b72c104b02ae69fbf8b29fefaa_Out_2.xxxx), _SampleTexture2D_e667746249694c549085be8edc862a6a_RGBA_0, (_Property_2dc11350183641b1976e0cb3796b76ff_Out_0.xxxx), _Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3);
    float _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0 = _AlphaClip;
    surface.BaseColor = (_Lerp_f1f5fd2c1a96496086977b2426ab343c_Out_3.xyz);
    surface.Emission = float3(0, 0, 0);
    surface.Alpha = _SampleTexture2D_e667746249694c549085be8edc862a6a_A_7;
    surface.AlphaClipThreshold = _Property_c9cfd039bb5f4ad5a4f313949ec32be1_Out_0;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs
#ifdef HAVE_VFX_MODIFICATION
#define VFX_SRP_ATTRIBUTES AttributesMesh
#define VaryingsMeshType VaryingsMeshToPS
#define VFX_SRP_VARYINGS VaryingsMeshType
#define VFX_SRP_SURFACE_INPUTS FragInputs
#endif
SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    #if defined(SHADER_STAGE_RAY_TRACING)
    #else
    #endif
    output.uv0 = input.texCoord0;

    // splice point to copy frag inputs custom interpolator pack into the SDI
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */

    return output;
}

// --------------------------------------------------
// Build Surface Data (Specific Material)

void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
{
    // setup defaults -- these are used if the graph doesn't output a value
    ZERO_INITIALIZE(SurfaceData, surfaceData);

    // copy across graph values, if defined
    surfaceData.color = surfaceDescription.BaseColor;

    #ifdef WRITE_NORMAL_BUFFER
    // When we need to export the normal (in the depth prepass, we write the geometry one)
    surfaceData.normalWS = fragInputs.tangentToWorld[2];
    #endif

    #if defined(DEBUG_DISPLAY)
    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
    {
        // TODO
    }
    #endif

    #ifdef _ENABLE_SHADOW_MATTE

        #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)

            HDShadowContext shadowContext = InitShadowContext();

            // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
            float3 shadow3;
            ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLightLayer(), shadow3);

            // Compute the average value in the fourth channel
            float4 shadow = float4(shadow3, dot(shadow3, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0)));

            float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
            float  localAlpha = saturate(shadowColor.a + surfaceDescription.Alpha);

            // Keep the nested lerp
            // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
            // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
            #ifdef _SURFACE_TYPE_TRANSPARENT
                surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
            #else
                surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
            #endif
            localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;

            surfaceDescription.Alpha = localAlpha;

        #elif SHADERPASS == SHADERPASS_PATH_TRACING

            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;

        #endif

    #endif // _ENABLE_SHADOW_MATTE
}

// --------------------------------------------------
// Get Surface And BuiltinData

void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
{
    // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
    #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
    #endif
    #endif

    #ifndef SHADER_UNLIT
    #ifdef _DOUBLESIDED_ON
        float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
    #else
        float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
    #endif

    ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
    #endif // SHADER_UNLIT

    SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);

    #if defined(HAVE_VFX_MODIFICATION)
    GraphProperties properties;
    ZERO_INITIALIZE(GraphProperties, properties);

    GetElementPixelProperties(fragInputs, properties);

    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
    #else
    SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
    #endif

    // Perform alpha test very early to save performance (a killed pixel will not sample textures)
    // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
    #ifdef _ALPHATEST_ON
        float alphaCutoff = surfaceDescription.AlphaClipThreshold;
        #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
        // The TransparentDepthPrepass is also used with SSR transparent.
        // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
        // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
        #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
        // DepthPostpass always use its own alpha threshold
        alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
        #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
        // If use shadow threshold isn't enable we don't allow any test
        #endif

        GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
    #endif

    #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
    #endif

    #ifndef SHADER_UNLIT
    float3 bentNormalWS;
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

    // Builtin Data
    // For back lighting we use the oposite vertex normal
    InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

    #else
    BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);

    ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
    builtinData.opacity = surfaceDescription.Alpha;

    #if defined(DEBUG_DISPLAY)
    // Light Layers are currently not used for the Unlit shader (because it is not lit)
    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
    // display in the light layers visualization mode, therefore we need the renderingLayers
    builtinData.renderingLayers = GetMeshRenderingLightLayer();
#endif

#endif // SHADER_UNLIT

#ifdef _ALPHATEST_ON
    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
    builtinData.alphaClipTreshold = alphaCutoff;
#endif

    // override sampleBakedGI - not used by Unlit

    builtinData.emissiveColor = surfaceDescription.Emission;

    // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
    // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
    // This is a limitation of the current MRT approach.
    #ifdef UNITY_VIRTUAL_TEXTURING
    #endif

    #if _DEPTHOFFSET_ON
    builtinData.depthOffset = surfaceDescription.DepthOffset;
    #endif

    // TODO: We should generate distortion / distortionBlur for non distortion pass
    #if (SHADERPASS == SHADERPASS_DISTORTION)
    builtinData.distortion = surfaceDescription.Distortion;
    builtinData.distortionBlur = surfaceDescription.DistortionBlur;
    #endif

    #ifndef SHADER_UNLIT
    // PostInitBuiltinData call ApplyDebugToBuiltinData
    PostInitBuiltinData(V, posInput, surfaceData, builtinData);
    #else
    ApplyDebugToBuiltinData(builtinData);
    #endif

    RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
}

// --------------------------------------------------
// Main

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassPathTracing.hlsl"

// --------------------------------------------------
// Visual Effect Vertex Invocations

#ifdef HAVE_VFX_MODIFICATION
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
#endif

ENDHLSL
}
}
CustomEditorForRenderPipeline "Rendering.HighDefinition.HDUnlitGUI" "UnityEngine.Rendering.HighDefinition.HDRenderPipelineAsset"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
FallBack "Hidden/Shader Graph/FallbackError"
}