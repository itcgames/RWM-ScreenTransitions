using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class ScreenTransitionTests
    {
        private Camera mainCam;

        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }

        [TearDown]
        public void Teardown()
        {
            SceneManager.UnloadSceneAsync("SampleScene");
        }

        [UnityTest]
        public IEnumerator AddingPoints()
        {
            setupCamera();
            TransitionPoint point = new TransitionPoint();

            point.transitionPoint = new Vector2(1.0f, 0.0f);
            mainCam.GetComponent<ScreenTransition>().AddPoint(point);

            point = new TransitionPoint();

            point.transitionPoint = new Vector2(2.0f, 0.0f);
            mainCam.GetComponent<ScreenTransition>().AddPoint(point);
            yield return null;

            Assert.AreEqual(2, mainCam.GetComponent<ScreenTransition>().transitionPoints.Count);

        }

        [UnityTest]
        public IEnumerator RemovingLastAddedPoint()
        {
            setupCamera();

            TransitionPoint point = new TransitionPoint();
            point.transitionPoint = new Vector2(1.0f, 0.0f);
            mainCam.GetComponent<ScreenTransition>().AddPoint(point);

            point = new TransitionPoint();
            point.transitionPoint = new Vector2(2.0f, 0.0f);
            mainCam.GetComponent<ScreenTransition>().AddPoint(point);


            mainCam.GetComponent<ScreenTransition>().RemoveLastPoint();
            yield return null;

            Assert.AreEqual(1, mainCam.GetComponent<ScreenTransition>().transitionPoints.Count);

        }

        [UnityTest]
        public IEnumerator CameraMoves()
        {
            setupCamera();

            TransitionPoint point = new TransitionPoint();
            point.transitionPoint = new Vector2(3.0f, 0.0f);
            mainCam.GetComponent<ScreenTransition>().AddPoint(point);

            Vector3 beforeMove = mainCam.transform.position;

            mainCam.GetComponent<ScreenTransition>().BeginTransition(0);

            yield return new WaitForSeconds(0.1f);

            Assert.AreNotEqual(beforeMove, mainCam.transform.position);
            Assert.AreEqual(true, mainCam.GetComponent<ScreenTransition>().transitioning);

        }

        private void setupCamera()
        {
            mainCam = Camera.main;

            // remove any other points that may exist on the camera for our tests
            mainCam.GetComponent<ScreenTransition>().transitionPoints.Clear();
        }
    }
}