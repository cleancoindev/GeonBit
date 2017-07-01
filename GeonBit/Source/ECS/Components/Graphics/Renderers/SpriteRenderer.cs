﻿#region LICENSE
//-----------------------------------------------------------------------------
// For the purpose of making video games, educational projects or gamification,
// GeonBit is distributed under the MIT license and is totally free to use.
// To use this source code or GeonBit as a whole for other purposes, please seek 
// permission from the library author, Ronen Ness.
// 
// Copyright (c) 2017 Ronen Ness [ronenness@gmail.com].
// Do not remove this license notice.
//-----------------------------------------------------------------------------
#endregion
#region File Description
//-----------------------------------------------------------------------------
// A component that renders a texture always facing camera.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.Core.Graphics;
using GeonBit.Core.Graphics.Materials;

namespace GeonBit.ECS.Components.Graphics
{
    /// <summary>
    /// This component renders a 3D quad that always faces the active camera, with animated sprite on it.
    /// </summary>
    public class SpriteRenderer : BaseRendererComponent
    {
        /// <summary>
        /// The entity from the core layer used to draw the model.
        /// </summary>
        protected SpriteEntity _entity;

        /// <summary>
        /// Get the main entity instance of this renderer.
        /// </summary>
        protected override BaseRenderableEntity Entity { get { return _entity; } }

        /// <summary>
        /// Playing animations speed (relevant if using animation clips).
        /// </summary>
        public float AnimationSpeed = 1f;

        /// <summary>
        /// Override material default settings for this specific model instance.
        /// </summary>
        public MaterialOverrides MaterialOverride
        {
            get { return _entity.MaterialOverride; }
            set { _entity.MaterialOverride = value; }
        }

        /// <summary>
        /// Set / get optional axis to lock rotation to.
        /// </summary>
        public Vector3? LockedAxis
        {
            get { return _entity.LockedAxis; }
            set { _entity.LockedAxis = value; }
        }

        /// <summary>
        /// Set / get the material of this sprite.
        /// </summary>
        public MaterialAPI Material
        {
            get { return _entity.Material; }
            set { _entity.Material = value; }
        }

        /// <summary>
        /// Create the sprite renderer component.
        /// </summary>
        /// <param name="spritesheet">Spritesheet data.</param>
        /// <param name="texture">Texture to use for this sprite with a default material.</param>
        /// <param name="useSharedMaterial">If true, will use a shared material for all sprites using this texture. 
        /// This optimization reduce load/unload time and memory, but it means the material is shared with other sprites.</param>
        public SpriteRenderer(SpriteSheet spritesheet, Texture2D texture = null, bool useSharedMaterial = true)
        {
            _entity = new SpriteEntity(spritesheet, texture, useSharedMaterial);
        }

        /// <summary>
        /// Create the sprite renderer component.
        /// </summary>
        /// <param name="spritesheet">Spritesheet data.</param>
        /// <param name="material">Material to use with this sprite.</param>
        public SpriteRenderer(SpriteSheet spritesheet, MaterialAPI material)
        {
            _entity = new SpriteEntity(spritesheet, material);
        }

        /// <summary>
        /// Create the sprite renderer component.
        /// </summary>
        /// <param name="spritesheet">Spritesheet data.</param>
        /// <param name="texturePath">Texture to use for this sprite with a new default material.</param>
        /// <param name="useSharedMaterial">If true, will use a shared material for all sprites using this texture. 
        /// This optimization reduce load/unload time and memory, but it means the material is shared with other sprites.</param>
        public SpriteRenderer(SpriteSheet spritesheet, string texturePath = null, bool useSharedMaterial = true) : 
            this(spritesheet, Resources.GetTexture(texturePath), useSharedMaterial)
        {
        }

        /// <summary>
        /// Play animation clip.
        /// </summary>
        /// <param name="clip">Animation clip to play.</param>
        /// <param name="speed">Animation playing speed.</param>
        public void PlayAnimation(SpriteAnimationClip clip, float speed = 1f)
        {
            _entity.PlayAnimation(clip, speed);
        }

        /// <summary>
        /// Change the spritesheet and current step of this sprite.
        /// </summary>
        /// <param name="newSpritesheet">New spritesheet data to use.</param>
        /// <param name="startingStep">Step to set from new spritesheet.</param>
        public void ChangeSpritesheet(SpriteSheet newSpritesheet, int startingStep = 0)
        {
            _entity.ChangeSpritesheet(newSpritesheet, startingStep);
        }

        /// <summary>
        /// Set spritesheet step from string identifier.
        /// </summary>
        /// <param name="identifier">Step identifier to set.</param>
        public void SetStep(string identifier)
        {
            _entity.SetStep(identifier);
        }

        /// <summary>
        /// Set spritesheet step from index.
        /// </summary>
        /// <param name="index">Step index to set.</param>
        public void SetStep(int index)
        {
            _entity.SetStep(index);
        }

        /// <summary>
        /// Called every frame in the Update() loop.
        /// Note: this is called only if GameObject is enabled.
        /// </summary>
        protected override void OnUpdate()
        {
            _entity.Update(Managers.TimeManager.TimeFactor * AnimationSpeed);
        }

        /// <summary>
        /// Clone this component.
        /// </summary>
        /// <returns>Cloned copy of this component.</returns>
        override public BaseComponent Clone()
        {
            SpriteRenderer ret = new SpriteRenderer(_entity.Spritesheet, _entity.Material);
            CopyBasics(ret);
            ret._entity.CopyStep(_entity);
            ret.MaterialOverride = _entity.MaterialOverride.Clone();
            ret.LockedAxis = LockedAxis;
            ret.AnimationSpeed = AnimationSpeed;
            return ret;
        }
    }
}
