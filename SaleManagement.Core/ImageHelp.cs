using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SaleManagement.Core
{
    public class ImageHelp
    {
        public static byte[] GetByteImage(Image img)
        {

            byte[] bt = null;

            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);

                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流

                    bt = new byte[mostream.Length];

                    mostream.Position = 0;//设置留的初始位置

                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));

                }

            }

            return bt;
        }

        public static MemoryStream MakeThumbnail(Stream originalImageStream, int dHeight, int dWidth, string path)
        {
            Image iSource = Image.FromStream(originalImageStream); ;//从指定的文件创建Image
            ImageFormat tFormat = iSource.RawFormat;//指定文件的格式并获取
            int sW = 0, sH = 0;//记录宽度和高度
            Size temsize = new Size(iSource.Width, iSource.Height);//实例化size。知矩形的高度和宽度
            if (temsize.Height > dHeight || temsize.Width > dWidth)//判断原图大小是否大于指定大小
            {
                if ((temsize.Width * dHeight) > (temsize.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * temsize.Height) / temsize.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (temsize.Width * dHeight) / temsize.Height;
                }
            }
            else//如果原图大小小于指定的大小
            {
                sW = temsize.Width;//原图宽度等于指定宽度
                sH = temsize.Height;//原图高度等于指定高度
            }
            Bitmap oB = new Bitmap(dWidth, dHeight);//实例化
            Graphics g = Graphics.FromImage(oB);//从指定的Image中创建Graphics
            g.Clear(Color.White);//设置画布背景颜色
            g.CompositingQuality = CompositingQuality.HighQuality;//合成图像的呈现质量
            g.SmoothingMode = SmoothingMode.HighQuality;//呈现质量
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//插补模式
                                                                       //开始重新绘制图像
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();//释放资源
                        //保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();//用于向图像编码器传递值
            long[] qy = new long[1];
            qy[0] = 100;
            EncoderParameter eParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParm;
            try
            {
                //获得包含有关内置图像编码器的信息的ImageCodeInfo对象
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo jpegICIinfo = null;
                MemoryStream thumbnailStream = new MemoryStream();
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    oB.Save(thumbnailStream, jpegICIinfo, ep);

                    oB.Save(path, jpegICIinfo, ep);

                }
                else
                {
                    oB.Save(thumbnailStream, tFormat);
                    oB.Save(path, tFormat);// 已指定格式保存到指定文件
                }

                return thumbnailStream;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                iSource.Dispose();//释放资源
                oB.Dispose();
            }
        }

    }
}
