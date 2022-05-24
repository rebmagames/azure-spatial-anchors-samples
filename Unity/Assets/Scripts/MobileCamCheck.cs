using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class MobileCamCheck : MonoBehaviour
{
    [SerializeField] private GameObject _cameraBlockedCanvas;

    [SerializeField]  private Texture2D _firstCameraTexture;

    private bool _retakePicture;
    private int _simalairColorsCount;


    public Material cam;

    private void Start()
    {
        InvokeStart();
    }
    public void InvokeStart()
    {
        if (_cameraBlockedCanvas == true)
        {
            //OnCamStart();
            InvokeRepeating("OnCamStart", 2.0f, 4f);
        }
    }
    public void OnCamStart()
    {
        
            _firstCameraTexture = ScreenCapture.CaptureScreenshotAsTexture();
        Debug.Log("Texture set###");
            //cam.mainTexture = _firstCameraTexture;
            
            StartCoroutine(GetPixelRowInfo());
        
        
    }
    private IEnumerator GetPixelRowInfo()
    {
        int pictureHeight = _firstCameraTexture.height / 2;

        int firstRowAmountOfTooSimilarColors = AnalysePixelRow(0, pictureHeight / 2 + pictureHeight, _firstCameraTexture.width, 1);
        yield return new WaitForEndOfFrame();
        int secondRowAmountOfTooSimilarColors = AnalysePixelRow(0, pictureHeight, _firstCameraTexture.width, 1);
        yield return new WaitForEndOfFrame();
        int thirdRowAmountOfTooSimilarColors = AnalysePixelRow(0, pictureHeight / 2, _firstCameraTexture.width, 1);
        yield return new WaitForEndOfFrame();

        Debug.Log($"###{firstRowAmountOfTooSimilarColors} colors were to close at 75% height. {secondRowAmountOfTooSimilarColors} colors were to close at 50% height. {thirdRowAmountOfTooSimilarColors} colors were to close at 25% height");

        if (firstRowAmountOfTooSimilarColors > 15 && secondRowAmountOfTooSimilarColors > 15 && thirdRowAmountOfTooSimilarColors > 13)
        {
            Debug.Log("###Photo is too similar, retaking it");
            Instantiate(_cameraBlockedCanvas);
            _retakePicture = true;
            _simalairColorsCount +=1;
            if(_simalairColorsCount == 1)
            {
                Debug.Log("### Pleas free the camera!");
                AnchorInfo.Instance.roomManager.SwitchCamBlocked(true);
                _simalairColorsCount = 0;
                _cameraBlockedCanvas.SetActive(false);
                CancelInvoke();
            }
        }
    }



    private int AnalysePixelRow(int HorizontalStartingPoint, int VerticalStartingPoint, int Width, int Height)
    {
        Color[] latestCameraTexture = _firstCameraTexture.GetPixels(HorizontalStartingPoint, VerticalStartingPoint, Width, Height);

        List<Color> averageColorPerSegment = new List<Color>();

        int pixelWithSegmentToCompare = latestCameraTexture.Length / 16;

        for (int i = 0; i < 16; i++)
        {
            float red = 0;
            float green = 0;
            float blue = 0;

            // i + 1 is to compare the next segment with the current segment
            for (int j = i * pixelWithSegmentToCompare; j < (i + 1) * pixelWithSegmentToCompare; j++)
            {
                red += latestCameraTexture[j].r;
                green += latestCameraTexture[j].g;
                blue += latestCameraTexture[j].b;
            }

            Color averageColor = new Color(red / pixelWithSegmentToCompare, green / pixelWithSegmentToCompare, blue / pixelWithSegmentToCompare);
            averageColorPerSegment.Add(averageColor);
        }

        int ColorsAreToSimilarCounter = 0;

        //cross compare average of the colors
        foreach (Color color in averageColorPerSegment)
        {
            if (ColorsInRange(color, averageColorPerSegment))
                ColorsAreToSimilarCounter++;
        }

        return ColorsAreToSimilarCounter;
    }

    /// <summary>
    /// Cross compares a color in the list with all the other colors in the list
    /// </summary>
    private bool ColorsInRange(Color ColorToCompare, List<Color> averageColors)
    {
        int colorsInRange = 0;
        bool halfOrMoreColorsWereInRange = false;

        for (int i = 0; i < averageColors.Count; i++) //one will always be itself
        {
            float range = 0.08f;
            bool redInRange = false;
            bool greenInRange = false;
            bool blueInRange = false;

            float redMin = averageColors[i].r - range;
            float redMax = averageColors[i].r + range;
            float greenMin = averageColors[i].g - range;
            float greenMax = averageColors[i].g + range;
            float blueMin = averageColors[i].b - range;
            float blueMax = averageColors[i].b + range;

            if (ColorToCompare.r > redMin && ColorToCompare.r < redMax)
                redInRange = true;
            if (ColorToCompare.g > greenMin && ColorToCompare.g < greenMax)
                greenInRange = true;
            if (ColorToCompare.b > blueMin && ColorToCompare.b < blueMax)
                blueInRange = true;

            if (redInRange && greenInRange && blueInRange)
                colorsInRange++;
        }

        if (colorsInRange > 8)
            halfOrMoreColorsWereInRange = true;

        return halfOrMoreColorsWereInRange;
    }
}