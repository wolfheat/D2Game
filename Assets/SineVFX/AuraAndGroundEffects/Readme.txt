Aura and Ground Effects
Version 1.0 (30.12.2017)


IMPORTANT NOTES:

- Turn on "HDR" on your Camera, Shaders requires it
- This VFX Asset looks much better in "Linear Rendering", but there is also optimized Prefabs for "Gamma Rendering" Mode
- Image Effects are necessary in order to make a great looking game, as well as our asset. Be sure you using "Tone Mapping" and "Bloom"
- We also recommend using Deferred Rendering for better performance

PERFORMANCE TIPS:

- If you planning to use multiple Aura effects in a limited space, we highly recommend you to use prefabs from "ArrayAuras" folder
- You can turn off "Distortion Noise" and "Noise02" in material properties for better performance
- Reduce the size of the textures that are used in the material
- Turn On texture image compression

HOW TO USE:

First of all, check for Demo Scene in Scenes folder, all effects are located in a "CompleteEffects" empty GameObject. Also, there is a Prefabs folder with complete effects.
Just Drag and Drop prefabs from "Prefabs" folder into your scene, these effects are updating in the editor, so you can preview them in real-time.
We made all Shaders very tweakable, so you can create your own unique effects.


SHADERS CONTROL:

Affector Count - Used in Array shaders, defines how many affectors/sources your Array Aura will have
Final Power - Final brightness of the image, you need to lower this value if you using "Gamma Rendering" Mode

Mask Constant Thickness - making thickness constant, regardless of the size of the GameObject
Mask Thickness - Aura effect thickness
Mask Distance - You cant normally change this value, it depends on a scale of GameObject
Mask Multiply - Multiply mask but this value
Mak Exp - Power Value of the mask
Mask Texture Enabled - Enable the use of a grayscale gradient texture
Mask Texture - Grayscale gradient texture, used for creation of custom mask

Ramp - Ramp gradient texture, used for better coloring
Ramp Color Ting - multiply Ramp texture but this color
Ramp Multiply Tiling - Modifies Ramp texture and moving its colors
Ramp Flip - Flip the Ramp texture

Noise Distortion Power - Result power of noise distortion
Noise 01 - Main noise of the effects
Noise 02 - Aditional noise
Noise Mask Distortion - Noise texture, used for mask distortion




Support email: sinevfx@gmail.com