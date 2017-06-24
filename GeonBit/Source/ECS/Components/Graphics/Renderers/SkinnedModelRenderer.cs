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
// A component that renders a 3D model with skinned animation.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeonBit.ECS.Components.Graphics
{
    /// <summary>
    /// This component renders an animated skinned 3D model.
    /// </summary>
    public class SkinnedModelRenderer : ModelRenderer
    {
        /// <summary>
        /// Animation speed factor.
        /// </summary>
        public float AnimationSpeed = 1f;

        /// <summary>
        /// If true, will play current animation clip in a loop.
        /// </summary>
        public bool IsLooped = true;


        /// <summary>
        /// Lock animation while transitioning.
        /// </summary>
        public bool LockWhileTransitioning
        {
            get { return _skinnedEntity.LockWhileTransitioning; }
            set { _skinnedEntity.LockWhileTransitioning = true; }
        }

        /// <summary>
        /// Set / get transition time between clips, when changing animation clip.
        /// </summary>
        public float TransitionTime
        {
            get { return _skinnedEntity.TransitionTime; }
            set { _skinnedEntity.TransitionTime = value; }
        }

        /// <summary>
        /// Transition time to use when going back to idle animation after a clip ends.
        /// </summary>
        public float BackToIdleTransitionTime = 0.25f;

        /// <summary>
        /// The name of the clip to play in loop while no other clip is playing, or after finishing a one-timer clip.
        /// </summary>
        public string IdleAnimationClip = "";

        /// <summary>
        /// Return if currently playing the 'idle' animation.
        /// </summary>
        public bool IsIdle
        {
            get
            {
                return AnimationClip == IdleAnimationClip;
            }
        }

        /// <summary>
        /// Currently playing animation clip.
        /// </summary>
        public string AnimationClip
        {
            // get currently playing animation clip
            get
            {
                return _skinnedEntity.CurrentClipName;
            }

            // set current animation clip
            set
            {
                if (AnimationClip != value) { _skinnedEntity.SetClip(value); }
            }
        }

        /// <summary>
        /// Create the model renderer component.
        /// </summary>
        /// <param name="model">Path of the model asset to draw.</param>
        public SkinnedModelRenderer(string model) : this(Resources.GetModel(model))
        {
            // register the animation-end callback
            _skinnedEntity.OnAnimationEnds = () =>
            {
                OnAnimationEnd();
            };

            // set default idle
            IdleAnimationClip = AnimationClip;
        }

        /// <summary>
        /// Set the currently playing animation clip.
        /// </summary>
        /// <param name="identifier">The identifier of the animation clip.</param>
        /// <param name="forceRestart">If false and already playing current animation, will not restart it.</param>
        /// <param name="inLoop">If true, will play clip animation in loop. If false will play once and return to idle animation.</param>
        /// <param name="transitionTime">Will set animation transition time for current animation.</param>
        public void SetClip(string identifier, bool forceRestart = false, bool inLoop = true, float transitionTime = 0.5f)
        {
            // set if playing in loop and transition time
            IsLooped = inLoop;
            TransitionTime = transitionTime;

            // if changing animation or forced to restart a new animation, set clip
            if (forceRestart || AnimationClip != identifier)
            {
                _skinnedEntity.SetClip(identifier);
            }
        }

        /// <summary>
        /// Called when current animation clip ended.
        /// </summary>
        private void OnAnimationEnd()
        {
            // if not looped, switch back to idle animation
            if (!IsLooped && IdleAnimationClip != null)
            {
                SetClip(identifier: IdleAnimationClip, forceRestart: false, inLoop: true, transitionTime: BackToIdleTransitionTime);
            }
        }

        /// <summary>
        /// Get the core entity as a skinned model entity.
        /// </summary>
        protected Core.Graphics.SkinnedModelEntity _skinnedEntity
        {
            get { return _entity as Core.Graphics.SkinnedModelEntity; }
        }

        /// <summary>
        /// Create the model renderer component.
        /// </summary>
        /// <param name="model">Model to draw.</param>
        public SkinnedModelRenderer(Model model)
        {
            _entity = new Core.Graphics.SkinnedModelEntity(model);
        }

        /// <summary>
        /// Called every frame in the Update() loop.
        /// Note: this is called only if GameObject is enabled.
        /// </summary>
        protected override void OnUpdate()
        {
            // create the skinned entity
            _skinnedEntity.Update(Managers.TimeManager.TimeFactor * AnimationSpeed, Matrix.Identity);
        }

        /// <summary>
        /// Clone this component.
        /// </summary>
        /// <returns>Cloned copy of this component.</returns>
        override public BaseComponent Clone()
        {
            SkinnedModelRenderer ret = new SkinnedModelRenderer(_entity.Model);
            CopyBasics(ret);
            ret.AnimationClip = AnimationClip;
            ret.AnimationSpeed = AnimationSpeed;
            ret.IsLooped = IsLooped;
            ret.TransitionTime = TransitionTime;
            ret.IdleAnimationClip = IdleAnimationClip;
            ret.BackToIdleTransitionTime = BackToIdleTransitionTime;
            ret.LockWhileTransitioning = LockWhileTransitioning;
            return ret;
        }
    }
}