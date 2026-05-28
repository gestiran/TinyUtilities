// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityRandom = UnityEngine.Random;

namespace TinyUtilities.Extensions.Unity {
    public static class ParticleSystemExtension {
        public static ParticleSystem PlayInstance(this ParticleSystem particle, Vector3 position, Transform parent = null) {
            Transform particleTransform = particle.transform;
            ParticleSystem instance = UnityObject.Instantiate(particle, position + particleTransform.position, particleTransform.rotation, parent);
            instance.Play();
            return instance;
        }
        
        public static ParticleSystem ScaleTo(this ParticleSystem particle, Vector3 localScale) {
            particle.transform.ScaleTo(localScale);
            return particle;
        }
        
        public static ParticleSystem DestroyAfter(this ParticleSystem particle, float seconds) {
            particle.gameObject.DestroyAfter(seconds);
            return particle;
        }
        
        public static void Play<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Play(withChildren);
            }
        }
        
        public static void Stop<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Stop(withChildren);
            }
        }
        
        public static void Clear<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Clear(withChildren);
            }
        }
        
        public static void PlayWhenStop(this ParticleSystem particle, bool withChildren = false) {
            if (particle.isPlaying == false) {
                particle.Play(withChildren);
            }
        }
        
        public static void StopWhenActive(this ParticleSystem particle, bool withChildren = false) {
            if (particle.isPlaying) {
                particle.Stop(withChildren);
            }
        }
        
        [Pure]
        public static ParticleSystem.Particle[] Burst(this ParticleSystem root, int size) {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[size];
            root.Burst(particles, particles.Length, 0);
            return particles;
        }
        
        public static void Burst(this ParticleSystem root, ParticleSystem.Particle[] particles, int size, int offset) {
            ParticleSystem.MainModule main = root.main;
            ParticleSystem.ShapeModule shape = root.shape;
            Transform transform = root.transform;
            
            size += offset;
            
            for (int i = offset; i < size; i++) {
                ParticleSystem.Particle particle = new ParticleSystem.Particle();
                
                particle.startLifetime = EvaluateCurve(main.startLifetime);
                particle.remainingLifetime = particle.startLifetime;
                particle.startSize = EvaluateCurve(main.startSize);
                particle.startColor = EvaluateGradient(main.startColor);
                particle.rotation = EvaluateCurve(main.startRotation);
                
                GetShapePositionAndDirection(shape, out Vector3 localPosition, out Vector3 localVelocity);
                
                localVelocity *= EvaluateCurve(main.startSpeed);
                
                if (main.simulationSpace == ParticleSystemSimulationSpace.World) {
                    particle.position = transform.TransformPoint(localPosition);
                    particle.velocity = transform.TransformDirection(localVelocity);
                } else if (main.simulationSpace == ParticleSystemSimulationSpace.Custom && main.customSimulationSpace != null) {
                    particle.position = main.customSimulationSpace.InverseTransformPoint(transform.TransformPoint(localPosition));
                    particle.velocity = main.customSimulationSpace.InverseTransformDirection(transform.TransformDirection(localVelocity));
                } else {
                    particle.position = localPosition;
                    particle.velocity = localVelocity;
                }
                
                particle.randomSeed = (uint)UnityRandom.Range(0, int.MaxValue);
                
                particles[i] = particle;
            }
        }
        
        private static void GetShapePositionAndDirection(ParticleSystem.ShapeModule shape, out Vector3 position, out Vector3 direction) {
            position = Vector3.zero;
            direction = Vector3.up;
            
            if (!shape.enabled) {
                return;
            }
            
            switch (shape.shapeType) {
                case ParticleSystemShapeType.Sphere: {
                    Vector3 onSphere = UnityRandom.onUnitSphere;
                    float minR = shape.radius * (1f - shape.radiusThickness);
                    float maxR = shape.radius;
                    position = onSphere * UnityRandom.Range(minR, maxR);
                    direction = onSphere;
                    break;
                }
                
                case ParticleSystemShapeType.Hemisphere: {
                    Vector3 onSphere = UnityRandom.onUnitSphere;
                    onSphere.y = Mathf.Abs(onSphere.y);
                    float minR = shape.radius * (1f - shape.radiusThickness);
                    float maxR = shape.radius;
                    position = onSphere * UnityRandom.Range(minR, maxR);
                    direction = onSphere;
                    break;
                }
                
                case ParticleSystemShapeType.Cone:
                case ParticleSystemShapeType.ConeVolume: {
                    float angleRad = shape.angle * Mathf.Deg2Rad;
                    float minR = shape.radius * (1f - shape.radiusThickness);
                    float maxR = shape.radius;
                    
                    float theta = UnityRandom.value * Mathf.PI * 2f;
                    float rPoint = Mathf.Sqrt(UnityRandom.value) * (maxR - minR) + minR;
                    
                    Vector3 basePos = new Vector3(rPoint * Mathf.Cos(theta), 0f, rPoint * Mathf.Sin(theta));
                    
                    float spread = Mathf.Tan(angleRad);
                    direction = new Vector3(basePos.x * spread, 1f, basePos.z * spread).normalized;
                    
                    if (shape.shapeType == ParticleSystemShapeType.ConeVolume) {
                        position = basePos + direction * shape.length * UnityRandom.value;
                    } else {
                        position = basePos;
                    }
                    
                    break;
                }
                
                case ParticleSystemShapeType.Box:
                case ParticleSystemShapeType.BoxShell:
                case ParticleSystemShapeType.BoxEdge: {
                    Vector3 half = shape.scale * 0.5f;
                    
                    if (shape.shapeType == ParticleSystemShapeType.Box) {
                        position = new Vector3(UnityRandom.Range(-half.x, half.x), UnityRandom.Range(-half.y, half.y), UnityRandom.Range(-half.z, half.z));
                    } else if (shape.shapeType == ParticleSystemShapeType.BoxShell) {
                        position = RandomOnBoxSurface(half);
                    } else {
                        position = RandomOnBoxEdge(half);
                    }
                    
                    direction = Vector3.up;
                    break;
                }
                
                case ParticleSystemShapeType.Circle: {
                    float theta = UnityRandom.value * Mathf.PI * 2f;
                    float minR = shape.radius * (1f - shape.radiusThickness);
                    float r = UnityRandom.Range(minR, shape.radius);
                    position = new Vector3(r * Mathf.Cos(theta), 0f, r * Mathf.Sin(theta));
                    direction = Vector3.up;
                    break;
                }
                
                case ParticleSystemShapeType.SingleSidedEdge: {
                    position = new Vector3(UnityRandom.Range(-shape.radius, shape.radius), 0f, 0f);
                    direction = Vector3.up;
                    break;
                }
                
                case ParticleSystemShapeType.Rectangle: {
                    Vector3 half = shape.scale * 0.5f;
                    position = new Vector3(UnityRandom.Range(-half.x, half.x), UnityRandom.Range(-half.y, half.y), 0f);
                    direction = Vector3.forward;
                    break;
                }
                
                case ParticleSystemShapeType.Mesh: {
                    if (shape.mesh != null && shape.mesh.vertexCount > 0) {
                        Vector3[] verts = shape.mesh.vertices;
                        position = verts[UnityRandom.Range(0, verts.Length)];
                        direction = Vector3.up;
                    }
                    
                    break;
                }
                
                case ParticleSystemShapeType.SkinnedMeshRenderer: {
                    if (shape.skinnedMeshRenderer != null) {
                        Mesh baked = new Mesh();
                        shape.skinnedMeshRenderer.BakeMesh(baked);
                        Vector3[] verts = baked.vertices;
                        
                        if (verts.Length > 0) {
                            position = verts[UnityRandom.Range(0, verts.Length)];
                        }
                        
                        direction = Vector3.up;
                    }
                    
                    break;
                }
                
                default:
                    position = Vector3.zero;
                    direction = Vector3.up;
                    break;
            }
            
            position = Quaternion.Euler(shape.rotation) * position + shape.position;
        }
        
        
        private static float EvaluateCurve(ParticleSystem.MinMaxCurve curve) {
            switch (curve.mode) {
                case ParticleSystemCurveMode.Constant:
                    return curve.constant;
                
                case ParticleSystemCurveMode.TwoConstants:
                    return UnityRandom.Range(curve.constantMin, curve.constantMax);
                
                case ParticleSystemCurveMode.Curve:
                    return curve.curve.Evaluate(UnityRandom.value) * curve.curveMultiplier;
                
                case ParticleSystemCurveMode.TwoCurves:
                    float t = UnityRandom.value;
                    return Mathf.Lerp(curve.curveMin.Evaluate(t) * curve.curveMultiplier, curve.curveMax.Evaluate(t) * curve.curveMultiplier, UnityRandom.value);
                
                default:
                    return curve.constant;
            }
        }
        
        private static Color EvaluateGradient(ParticleSystem.MinMaxGradient gradient) {
            switch (gradient.mode) {
                case ParticleSystemGradientMode.Color:
                    return gradient.color;
                
                case ParticleSystemGradientMode.TwoColors:
                    return Color.Lerp(gradient.colorMin, gradient.colorMax, UnityRandom.value);
                
                case ParticleSystemGradientMode.Gradient:
                    return gradient.gradient.Evaluate(UnityRandom.value);
                
                case ParticleSystemGradientMode.TwoGradients:
                    return Color.Lerp(gradient.gradientMin.Evaluate(UnityRandom.value), gradient.gradientMax.Evaluate(UnityRandom.value), UnityRandom.value);
                
                case ParticleSystemGradientMode.RandomColor:
                    return gradient.gradient.Evaluate(UnityRandom.value);
                
                default:
                    return gradient.color;
            }
        }
        
        private static Vector3 RandomOnBoxSurface(Vector3 half) {
            float px = half.y * half.z;
            float py = half.x * half.z;
            float pz = half.x * half.y;
            float total = 2f * (px + py + pz);
            float r = UnityRandom.value * total;
            
            if (r < 2f * px) {
                return new Vector3(r < px ? -half.x : half.x, UnityRandom.Range(-half.y, half.y), UnityRandom.Range(-half.z, half.z));
            }
            
            r -= 2f * px;
            
            if (r < 2f * py) {
                return new Vector3(UnityRandom.Range(-half.x, half.x), r < py ? -half.y : half.y, UnityRandom.Range(-half.z, half.z));
            }
            
            r -= 2f * py;
            return new Vector3(UnityRandom.Range(-half.x, half.x), UnityRandom.Range(-half.y, half.y), r < pz ? -half.z : half.z);
        }
        
        private static Vector3 RandomOnBoxEdge(Vector3 half) {
            int edge = UnityRandom.Range(0, 12);
            float t = UnityRandom.Range(-1f, 1f);
            
            switch (edge) {
                case 0: return new Vector3(t * half.x, -half.y, -half.z);
                case 1: return new Vector3(t * half.x, -half.y, half.z);
                case 2: return new Vector3(t * half.x, half.y, -half.z);
                case 3: return new Vector3(t * half.x, half.y, half.z);
                case 4: return new Vector3(-half.x, t * half.y, -half.z);
                case 5: return new Vector3(-half.x, t * half.y, half.z);
                case 6: return new Vector3(half.x, t * half.y, -half.z);
                case 7: return new Vector3(half.x, t * half.y, half.z);
                case 8: return new Vector3(-half.x, -half.y, t * half.z);
                case 9: return new Vector3(-half.x, half.y, t * half.z);
                case 10: return new Vector3(half.x, -half.y, t * half.z);
                default: return new Vector3(half.x, half.y, t * half.z);
            }
        }
    }
}