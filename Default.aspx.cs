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

public partial class _Default : System.Web.UI.Page
{
    private const String folderName = "UserImages/";

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";         
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        FileUpload1.SaveAs(Server.MapPath(folderName) + FileUpload1.FileName);
        if (!String.IsNullOrEmpty(FileUpload1.FileName))
        {
            Start();
        }
        else
        {
            lblMsg.Text = "Please Select an Image.";
        }
    }

    private void Start()
    {
        string imgFile = folderName + FileUpload1.FileName;
        Stream inputStream = new MemoryStream();
        System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(imgFile));

        img.Save(inputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        Stream outputStream;
        int diameter;
        if (int.TryParse(tbDiameter.Text.Trim(), out diameter))
        {
            outputStream = CircleImageCreater.CreateCircleImageStream(inputStream, diameter);
        }
        else
        {
            outputStream = CircleImageCreater.CreateCircleImageStream(inputStream);
        }
      
        saveImage(outputStream, FileUpload1.FileName + ".png");

        oldImage.ImageUrl = imgFile;
        newImage.ImageUrl = folderName + FileUpload1.FileName + ".png";

        //dispose
        inputStream.Dispose();
        outputStream.Dispose();
    }

    //save images
    //private void saveImage(System.Drawing.Image img, string imgName)
    //{
    //    Bitmap Bmp = new Bitmap(img);
    //    saveImage(Bmp, imgName);
    //}
    private void saveImage(Stream imgStream, string imgName)
    {
        Bitmap Bmp = new Bitmap(imgStream);
        saveImage(Bmp, imgName);
    }
    private void saveImage(Bitmap Bmp, string imgName)
    {
        string spath = Server.MapPath(folderName);
        if (!System.IO.Directory.Exists(spath))
        {
            System.IO.Directory.CreateDirectory(spath);
        }

        Bmp.Save(Server.MapPath(folderName) + imgName);
        //ImageCodecInfo pngCodec = ImageCodecInfo.GetImageEncoders().Where(codec => codec.FormatID.Equals(ImageFormat.Png.Guid)).FirstOrDefault();
        //if (pngCodec != null)
        //{
        //    EncoderParameters parameters = new EncoderParameters();
        //    parameters.Param[0] = new EncoderParameter(Encoder.Quality, 24L);
        //    Bmp.Save(Server.MapPath("~/UserImages/") + @"\" + imgName, pngCodec, parameters);
        //}
        //Bmp.Dispose();
    }

    
}