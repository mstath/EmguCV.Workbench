using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Parse text from image using Tesseract OCR engine.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    [Export(typeof(IImageAlgorithm))]
    public class OpticalCharacterRecognition : ImageAlgorithm
    {
        private readonly Tesseract _ocr = new Tesseract(Environment.CurrentDirectory + @"\tessdata", "eng",
            OcrEngineMode.TesseractLstmCombined);

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            // set the image for OCR
            _ocr.SetImage(image.Convert<Gray, byte>());

            // attempt recognition, return if not recognized
            if (_ocr.Recognize() != 0) return;

            // get the recognized characters
            var characters = _ocr.GetCharacters();

            // draw bounding rectangle around each recognized character
            foreach (var character in characters)
                annotatedImage.Draw(character.Region, new Bgr(_annoColor.Color()), _lineThick);

            // return the recognized string as data object
            var text = _ocr.GetUTF8Text();
            data = new List<object> {new StringValue(text)};
        }
    }
}
