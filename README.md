# ABOUT #

This is essentially a WPF GUI wrapper to several image processors and algorithms from [Emgu CV](http://www.emgu.com), the .NET wrapper to [OpenCV](https://opencv.org/).  Features include:
- Image source from webcam or file
- An image processing module where you can add and order several processors with parameters exposed
- Several tutorial/example algorithms implemented with paremeters exposed
- Interactive drag-select for template and feature match algorithms
- Data grid updated per frame results
- Realtime feedback of parameter modification for webcam and file image sources

### What is this repository for? ###

This is to help you get up and going quickly with Emgu CV.  It is a mix of examples from the Emgu repo and OpenCV tutorials.

### How do I get set up? ###

- Implemented with Visual Studio 2015 and .NET 4.6.1
- Depends on NuGet packages for Emgu CV and Extended.Wpf.Toolkit