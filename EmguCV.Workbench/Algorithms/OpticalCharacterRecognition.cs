using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.Algorithms
{
    public class OpticalCharacterRecognition : ImageAlgorithm
    {
        private readonly Tesseract _ocr = new Tesseract(Environment.CurrentDirectory + @"\tessdata", "eng",
            OcrEngineMode.TesseractLstmCombined);

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            _ocr.SetImage(image.Convert<Gray, byte>());

            if (_ocr.Recognize() != 0) return;

            var characters = _ocr.GetCharacters();

            foreach (var character in characters)
                annotatedImage.Draw(character.Region, new Bgr(_annoColor.Color()), _lineThick);

            var text = _ocr.GetUTF8Text();
            data = new List<object> {new StringValue(text)};
        }
    }
}
