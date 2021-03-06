using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut09_HierarchyAndInput", Description = "Yet another FUSEE App.")]
    public class Tut09_HierarchyAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private float _camAngle = 0;
        private Transform _baseTransform;
        private Transform _bodyTransform;
        private Transform _upperArmTransform;
        private Transform _foreArmTransform;
        private Transform _grip1Transform;
        private Transform _grip2Transform;
        private Boolean opening = true;
        private float _clawAngle = 0.5f;
        private Boolean spacePressed = false;


       SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };

            _upperArmTransform = new Transform
            {
                Rotation = new float3(-1f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };

            _foreArmTransform = new Transform
            {
                Rotation = new float3(-1f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-2, 8, 0)
            };

            _grip1Transform = new Transform
            {
                Rotation = new float3(0.5f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 9, 1f)
            };

            _grip2Transform = new Transform
            {
                Rotation = new float3(-0.5f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 9, -1f)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                // grey base
                Children = new List<SceneNode>
                {
                    new SceneNode
                    {
                        Components = new List<SceneComponent>
                        {
                            // TRANSFORM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey, float4.Zero),

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        },
                        Children = {
                            // red
                            new SceneNode {
                                Components = {
                                    _bodyTransform,
                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.Red, float4.Zero),
                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                },
                                Children = {
                                    // green
                                new SceneNode
                                {
                                    Components =
                                    {
                                        _upperArmTransform,
                                    },
                                    Children = 
                                    {
                                        new SceneNode
                                        {
                                            Components = 
                                            {
                                                new Transform
                                                {
                                                    Rotation = new float3(0, 0, 0),
                                                    Scale = new float3(1, 1, 1),
                                                    Translation = new float3(0, 4, 0)
                                                },
                                                MakeEffect.FromDiffuseSpecular((float4) ColorUint.Green, float4.Zero),
                                                SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                            }
                                        },
                                        // blue
                                        new SceneNode
                                        {
                                            Components =
                                            {
                                                _foreArmTransform,
                                            },
                                            Children = 
                                            {
                                                new SceneNode
                                                {
                                                    Components = 
                                                    {
                                                        new Transform
                                                        {
                                                            Rotation = new float3(0, 0, 0),
                                                            Scale = new float3(1, 1, 1),
                                                            Translation = new float3(0, 4, 0)
                                                        },
                                                        MakeEffect.FromDiffuseSpecular((float4) ColorUint.Blue, float4.Zero),
                                                        SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                    }
                                                },

                                                new SceneNode
                                                {
                                                    Components =
                                                    {
                                                        _grip1Transform,
                                                        
                                                    },
                                                    Children = {
                                                        new SceneNode 
                                                        {
                                                            Components = 
                                                            {
                                                                new Transform
                                                                {
                                                                    Rotation = new float3(0, 0, 0),
                                                                    Scale = new float3(1, 1, 1),
                                                                    Translation = new float3(0, 1, 0)
                                                                },
                                                                MakeEffect.FromDiffuseSpecular((float4) ColorUint.Pink, float4.Zero),
                                                                SimpleMeshes.CreateCuboid(new float3(2, 2, 0.5f))
                                                            }
                                                        }
                                                    }
                                                },

                                                new SceneNode
                                                {
                                                    Components =
                                                    {
                                                        _grip2Transform,
                                                        
                                                    },
                                                    Children = {
                                                        new SceneNode 
                                                        {
                                                            Components = 
                                                            {
                                                                new Transform
                                                                {
                                                                    Rotation = new float3(0, 0, 0),
                                                                    Scale = new float3(1, 1, 1),
                                                                    Translation = new float3(0, 1, -0)
                                                                },
                                                                MakeEffect.FromDiffuseSpecular((float4) ColorUint.Pink, float4.Zero),
                                                                SimpleMeshes.CreateCuboid(new float3(2, 2, 0.5f))
                                                            }
                                                        }
                                                    }
                                                }
                                            
                                            }
                                        }
                                    }

                                }
                            }
                                
                            }
                        }
                    }
                }
            };
        }


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            _bodyTransform.Rotation.y += 1.5f * Time.DeltaTime * Keyboard.LeftRightAxis;
            _upperArmTransform.Rotation.x += 0.5f * Time.DeltaTime * Keyboard.UpDownAxis;
            _foreArmTransform.Rotation.x += 0.5f * Time.DeltaTime * Keyboard.WSAxis;

            _grip1Transform.Rotation.x = _clawAngle;
            _grip2Transform.Rotation.x = -_clawAngle;

            if (opening) {
                if (_clawAngle < 0.5f) {
                    _clawAngle += 0.5f * DeltaTime;
                }
            } else {
                if (_clawAngle > -0.5f) {
                    _clawAngle -= 0.5f * DeltaTime;
                }
            }

            if (Mouse.LeftButton) {
                _camAngle += Mouse.Velocity.x * -0.0005f;
            }

            if (Keyboard.GetKey(KeyCodes.Space)) {
                if (!spacePressed) {
                    opening = !opening;
                }
                spacePressed = true;
            } else {
                spacePressed = false;
            }

            SetProjectionAndViewport();
            
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        
    }
}