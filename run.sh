#!/bin/bash
if [[ `hostname` = "BladeStealth" ]]; then
    if [[ `glxinfo | grep OpenGL | grep NVIDIA` = "" ]]; then
        export __NV_PRIME_RENDER_OFFLOAD=1
        export __GLX_VENDOR_LIBRARY_NAME=nvidia
        export __VK_LAYER_NV_optimus=NVIDIA_only
        export VK_ICD_FILENAMES=/usr/share/vulkan/icd.d/nvidia_icd.json
    fi
fi
dotnet run
