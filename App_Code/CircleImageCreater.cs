using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CircleImageCreater
/// </summary>
public class CircleImageCreater
{
    private const int resizeScale = 4;
    private const int borderSize = resizeScale * 6;

	public CircleImageCreater()
	{

	}

    public static Stream CreateCircleImageStream(Stream imgStream)
    {
        //convert stream to bitmap
        Bitmap imgBitmap = new Bitmap(imgStream);
        int circleDiameter = imgBitmap.Width > imgBitmap.Height ? imgBitmap.Height : imgBitmap.Width;
        imgBitmap.Dispose();
        return CreateCircleImageStream(imgStream, circleDiameter);
    }

    public static Stream CreateCircleImageStream(Stream imgStream, int circleDiameter)
    {
        //convert stream to bitmap
        Bitmap imgBitmap = new Bitmap(imgStream);

        //for debug
        //saveImage(imgBitmap, "3 inputsteam to bitmap.jpg");

        //create squre image with circle drawn
        imgBitmap = CreateSqureImageAndDrawCicle(imgBitmap, circleDiameter * resizeScale);

        //create circle image
        imgBitmap = CropImageToCircle(imgBitmap);

        //for debug
        //saveImage(imgBitmap, "6 circle croped.jpg");   

        //reduce size
        System.Drawing.Image resizedImage = imgBitmap.GetThumbnailImage(circleDiameter, circleDiameter, null, System.IntPtr.Zero);

        //debug
        //saveImage(resizedImage, "7 resized.jpg");

        //convert bitmap to stream
        Stream outputStream = new MemoryStream();
        resizedImage.Save(outputStream, System.Drawing.Imaging.ImageFormat.Png);//jpg does not support transparency

        resizedImage.Dispose();
        imgBitmap.Dispose();

        return outputStream;
    }

    private static Bitmap CreateSqureImageAndDrawCicle(Bitmap fullSizeImg, int Size)
    {
        int width = fullSizeImg.Width;
        int height = fullSizeImg.Height;
        int x = 0;
        int y = 0;

        //Determine dimensions of resized version of the image 
        if (width > height)
        {
            width = (int)(width / height) * Size;
            height = Size;
            // moves cursor so that crop is more centered 
            x = Convert.ToInt32(Math.Ceiling((double)((width - height) / 2)));
        }
        else if (height > width)
        {
            height = (int)(height / width) * Size;
            width = Size;
            // moves cursor so that crop is more centered 
            y = Convert.ToInt32(Math.Ceiling((double)((height - width) / 2)));
        }
        else
        {
            width = Size;
            height = Size;
        }

        //First Resize the Existing Image 
        System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(width, height, null, System.IntPtr.Zero);

        //debug
        //saveImage(thumbNailImg, "4 thumbNailImg.jpg");

        //Clean up / Dispose... 
        fullSizeImg.Dispose();

        //Create a Crop Frame to apply to the Resized Image 
        Bitmap myBitmapCropped = new Bitmap(width, height);//PixelFormat.Format16bppRgb555
        myBitmapCropped.SetResolution(thumbNailImg.HorizontalResolution, thumbNailImg.VerticalResolution);

        Graphics myGraphic = FromImage(myBitmapCropped);

        //Apply the Crop to the Resized Image 
        myGraphic.DrawImage(thumbNailImg, new Rectangle(0, 0, myBitmapCropped.Width, myBitmapCropped.Height),
            x, y, myBitmapCropped.Width, myBitmapCropped.Height, GraphicsUnit.Pixel);

        //draw boarder
        myGraphic.DrawEllipse(new Pen(Color.White, borderSize),
            new Rectangle(0, 0, myBitmapCropped.Width, myBitmapCropped.Height));

        //Clean up / Dispose... 
        myGraphic.Dispose();

        //debug
        //saveImage(myBitmapCropped, "5 circle drawn.jpg");

        //Clean up / Dispose... 
        thumbNailImg.Dispose();
        return myBitmapCropped;
    }

    private static Bitmap CropImageToCircle(Bitmap SourceImage)
    {
        int circleDiameter = SourceImage.Width;
        int circleUpperLeftX = 0;
        int circleUpperLeftY = 0;

        //Load our source image
        //Create a rectangle that crops a square of our image
        Rectangle CropRect = new Rectangle(circleUpperLeftX, circleUpperLeftY, circleDiameter, circleDiameter);

        //Crop the image to that square
        using (Bitmap CroppedImage = SourceImage.Clone(CropRect, SourceImage.PixelFormat))
        {

            //Create a texturebrush to draw our circle with
            using (TextureBrush TB = new TextureBrush(CroppedImage))
            {

                //Create our output image
                Bitmap FinalImage = new Bitmap(circleDiameter, circleDiameter);

                //Create a graphics object to draw with
                using (Graphics myGraphic = FromImage(FinalImage))
                {
                    myGraphic.FillEllipse(TB, 0, 0, circleDiameter, circleDiameter);
                    return FinalImage;
                }
            }
        }
    }

    private static Graphics FromImage(System.Drawing.Image image)
    {
        Graphics myGraphic = Graphics.FromImage(image);
        myGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        myGraphic.SmoothingMode = SmoothingMode.AntiAlias;
        //myGraphic.CompositingQuality = CompositingQuality.HighQuality;
        myGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

        return myGraphic;
    }


    
    
}