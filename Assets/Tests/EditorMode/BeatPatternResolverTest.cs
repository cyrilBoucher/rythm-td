using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BeatPatternResolverTest
    {
        public BeatPatternResolver resolver;

        [SetUp]
        public void SetUp()
        {
            resolver = new BeatPatternResolver();
        }

        [Test, Combinatorial]
        public void BeatPatternResolverTest_ShouldResolveTwoElementsPattern(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Validated, result);
        }

        [Test]
        public void BeatPatternResolver_ShouldFailMissingSecondInput(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, BeatPattern.Input.Skip);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Waiting, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, BeatPattern.Input.Skip);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Missed, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldResolveThreeElementsPatternWithSkip(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(BeatPattern.Input.Skip);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, BeatPattern.Input.Skip);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset / 2.0f;

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Validated, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldResolveThreeElementsPatternWithNoSkip(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement3)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(patternElement2);
            pattern.pattern.Add(patternElement3);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, patternElement3);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Validated, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailMissingBeingEarlyOnLastInput(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(BeatPattern.Input.Skip);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, BeatPattern.Input.Skip);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = -(resolver.validationOffset + 1.0f);

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Early, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailMissingBeingLateOnLastInput(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(BeatPattern.Input.Skip);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, BeatPattern.Input.Skip);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            timeToClosestBeatSec = resolver.validationOffset + 1.0f;

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Late, result);
        }

        [Test]
        public void BeatPatternResolverTest_ShouldFailHittingInsteadOfSkipping(
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement1,
            [Values(BeatPattern.Input.Tap, BeatPattern.Input.SlideDown, BeatPattern.Input.SlideUp, BeatPattern.Input.SlideLeft, BeatPattern.Input.SlideRight)] BeatPattern.Input patternElement2)
        {
            BeatPattern pattern = new BeatPattern();
            pattern.pattern.Add(patternElement1);
            pattern.pattern.Add(BeatPattern.Input.Skip);
            pattern.pattern.Add(patternElement2);

            resolver.SetPattern(pattern);

            double timeToClosestBeatSec = resolver.validationOffset / 2.0f;
            BeatPatternResolver.ReturnType result = resolver.Run(timeToClosestBeatSec, patternElement1);

            Assert.AreEqual(BeatPatternResolver.ReturnType.Good, result);

            result = resolver.Run(timeToClosestBeatSec, patternElement2);

            Assert.AreEqual(BeatPatternResolver.ReturnType.WrongNote, result);
        }
    }
}
