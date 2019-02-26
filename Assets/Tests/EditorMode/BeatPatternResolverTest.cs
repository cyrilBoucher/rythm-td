using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BeatPatternResolverTest
    {
        [Test]
        public void BeatPatternResolverTest_ShouldResolveTwoElementsPattern()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Validated, result);
        }

        [Test]
        public void BeatPatternResolver_ShouldFailMissingSecondInputOnBeat()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            input = false;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Waiting, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Missed, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldResolveThreeElementsPattern()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.SkipBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;
            input = false;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            input = true;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Validated, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailMissingBeingEarlyOnLastInputOnBeat()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.SkipBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;
            input = false;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = -(resolver.validationOffset + 1.0f);
            input = true;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Early, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailMissingBeingLateOnLastInputOnBeat()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.SkipBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;
            input = false;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;
            input = true;

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Late, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailHittingInsteadOfSkipping()
        {
            BeatPatternResolver resolver = new BeatPatternResolver();

            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(BeatPattern.Input.OnBeat);
            pattern.pattern.Add(BeatPattern.Input.SkipBeat);
            pattern.pattern.Add(BeatPattern.Input.OnBeat);

            resolver.SetPattern(pattern);

            float timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            bool input = true;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, input);

            Assert.AreEqual(BeatPatternResolver.ReturnType.WrongNote, result);
        }
    }
}
