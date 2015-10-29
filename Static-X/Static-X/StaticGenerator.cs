using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Static_X
{
    class StaticGenerator
    {
        /// <summary>
        /// Binary enumeration of unique settings and combined preset settings
        /// </summary>
        [Flags]
        public enum Effect
        {
            /// <summary>
            /// No effect.
            /// </summary>
            None = 0x0,
            /// <summary>
            /// Turns on the red pixel component.
            /// </summary>
            Red = 0x1,
            /// <summary>
            /// Turns on the green pixel component.
            /// </summary>
            Green = 0x2,
            /// <summary>
            /// Turns on the blue pixel component.
            /// </summary>
            Blue = 0x4,
            /// <summary>
            /// Turns on the alpha pixel component.
            /// </summary>
            Alpha = 0x8,
            /// <summary>
            /// Locks the red pixel component value with all other locked components.
            /// </summary>
            RedLock = 0x10,
            /// <summary>
            /// Locks the green pixel component value with all other locked components.
            /// </summary>
            GreenLock = 0x20,
            /// <summary>
            /// Locks the blue pixel component value with all other locked components.
            /// </summary>
            BlueLock = 0x40,
            /// <summary>
            /// Locks the alpha pixel component value with all other locked components
            /// </summary>
            AlphaLock = 0x80,
            /// <summary>
            /// Forces the random pixel values to be selected as either 0 or 255
            /// </summary>
            Absolute = 0x100,
            /// <summary>
            /// Sets the default color compent value to 255 (Normally 0) [Does not affect alpha]
            /// </summary>
            Invert = 0x200,
            /// <summary>
            /// Turns on the scanning effect
            /// </summary>
            Scanning = 0x400,
            /// <summary>
            /// Preset - Red,Green,Blue,MaxAlpha
            /// </summary>
            Color = 0x7,
            /// <summary>
            /// Preset - Red,Green,Blue,Alpha
            /// </summary>
            AlphaColor = 0xF,
            /// <summary>
            /// Preset - Red,Green,Blue,RedLock,GreenLock,BlueLock,MaxAlpha
            /// </summary>
            GrayScale = 0x77,
            /// <summary>
            /// Preset - Red,Green,Blue,RedLock,GreenLock,BlueLock,Alpha
            /// </summary>
            AlphaGrayScale = 0x7F,
            /// <summary>
            /// Preset - Red,Green,Blue,RedLock,GreenLock,BlueLock,Absolute,MaxAlpha
            /// </summary>
            BlackAndWhite = 0x177,
            /// <summary>
            /// Easter Egg :)
            /// </summary>
            Secret = 0x8000
        }

        /// <summary>
        /// Historical. This enum is kept for backwards compatibility.
        /// It is used by NextFrameOld2 Pixel.
        /// </summary>
        /// 
        [Obsolete("Use Effect enum instead. Use this enum only for compatibility with older NextFrame methods")]
        [Flags]
        public enum Format
        {
            BlackAndWhite = 0x0,
            GrayScale = 0x1,
            Color = 0x2,
            ColorAndAlpha = 0x4
        }

        /// <summary>
        /// A representation of the pixel component placeholders in a Bitmap byte array formated at 32bits
        /// </summary>
        public enum Pixel
        {
            Blue = 0x0,
            Green = 0x1,
            Red = 0x2,
            Alpha = 0x3
        }

        private const int MILLISECONDS_PER_SECOND = 1000;
        private const int TICKS_PER_MILLISECOND = 10000;
        private const int TICKS_PER_SECOND = 10000000;

        private Control control;//Control that static will be painted on
        private Timer timer;//Timer that controls the evolution of frames
        private Random random;//Random number generator
#pragma warning disable CS0618
        private Format format;//HISTORICAL
#pragma warning restore CS0618
        private Effect effect;
        /// <summary>
        /// Stores a flag that indicates the type of static to generate.
        /// </summary>
        public Effect Effects
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }
#pragma warning disable CS0169
        private int resolution;//Not implemented
#pragma warning restore CS0169
        /// <summary>
        /// Not implemented. Will affect pixels size when implemented.
        /// </summary>
        /// <exception cref="NotImplementedException">Not implemented</exception>
        public int Resolution
        {
            get
            {
                throw new NotImplementedException();
                //return resolution;
            }
            set
            {
                throw new NotImplementedException();
                //if (value >= 1)
                //{
                //    resolution = value;
                //}
                //else
                //{
                //    throw new Exception("Resolution must be equal to or greater than one.");
                //}
            }
        }
        /// <summary>
        /// Gets the state of the program, true if running.
        /// </summary>
        public bool Running { get; private set; }
        /// <summary>
        /// Gets or sets the target frames per second.
        /// </summary>
        public double FramesPerSecond { get; set; }
        /// <summary>
        /// Gets or sets ShowDetails. When true, will display frame details on screen.
        /// </summary>
        public bool ShowDetails { get; set; }
        private int ScanSize { get; set; }//Size (height) in pixels of the scan effect
        private int scanLocation;//Current location of the scan effect
        private int controlArea;//Number of pixels that are paintable
        private int special;//This is a secret


        /// <summary>
        /// Initializes a new instance of the StaticGenerator class.
        /// </summary>
        /// <param name="control">The System.Windows.Forms Control that will be used to paint a static image on.</param>
        public StaticGenerator(Control control)
        {
            this.control = control;
            FramesPerSecond = 30;
            ScanSize = 30;
            controlArea = control.Width * control.Height;
            //HISTORICAL NOTE:
            //      Format is kept for backwards compatibility
            //      for the use of "NextFrameOld2()"
#pragma warning disable CS0618
            format = Format.BlackAndWhite;
#pragma warning restore CS0618
            effect = Effect.AlphaGrayScale;

            Initialize();
        }

        /// <summary>
        /// Initializes the timing function of this class.
        /// </summary>
        private void Initialize()
        {
            DateTime lastIterationTime = DateTime.Now;

            double totalFrameRate = 0;
            double frameRate = 0;
            double averageFrameRate = 0;
            long totalFrames = 1;
            scanLocation = 0;
            random = new Random((int)DateTime.Now.Ticks); ;

            timer = new Timer();
            //Set interval to fraction of a second by frames per second
            timer.Interval = (int)(MILLISECONDS_PER_SECOND / FramesPerSecond);
            //Set the method to execute each timer tick
            timer.Tick += (object sender, EventArgs e) =>
            {
                if (control != null && control.Width != 0 && control.Height != 0)
                {
                    //Create double buffered graphics elements
                    Bitmap offScreenBmp = new Bitmap(control.Width, control.Height);
                    Graphics offScreenDC = Graphics.FromImage(offScreenBmp);
                    Graphics clientDC = control.CreateGraphics();

                    //Set the start time of this tick
                    DateTime start = DateTime.Now;
                    //Records the number of ticks elapsed since the start of the last frame
                    double frameTicks = start.Subtract(lastIterationTime).Ticks;

                    //Request the next frame and draw to the off screen graphics control
                    //HISTORICAL NOTE:
                    //     To use historical frame methods, replace "NextFrame()"
                    //     with "NextFrameOld()" or "NextFrameOld2()".
                    offScreenDC.DrawImage(NextFrame(), new Point(0, 0));

                    //Collects and calculates frame details (Frame rate, average frame rate, elapsed frmaes)
                    frameRate = TICKS_PER_SECOND / frameTicks;
                    totalFrameRate += frameRate;
                    averageFrameRate = totalFrameRate / totalFrames;

                    //Displays frame details (Frame rate, average frame rate, elapsed frmaes)
                    if (ShowDetails)
                    {
                        string screenText = "Frames/Second:  " + frameRate.ToString("N2") + "\n" +
                                            "Average FPS:    " + averageFrameRate.ToString("N2") + "\n" +
                                            "Total Frames:   " + totalFrames;

                        Rectangle backRectangle = new Rectangle(new Point(0, 0), new Size(250, 48));

                        offScreenDC.FillRectangle(new SolidBrush(Color.White), backRectangle);
                        offScreenDC.DrawString(screenText, new Font(FontFamily.GenericMonospace, 10f, FontStyle.Bold), Brushes.Black, new PointF(2f, 2f));
                    }

                    clientDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    clientDC.DrawImage(offScreenBmp, 0, 0);

                    //Marks the end of this frame's processing time
                    DateTime end = DateTime.Now;
                    //Calculates the target elapsed time between frames in milliseconds
                    int targetMillisecondPerFrame = (int)(MILLISECONDS_PER_SECOND / FramesPerSecond);
                    //Calcualtes the total number of ticks elapsed in this frame
                    double elapsedTicks = end.Subtract(start).Ticks;
                    //Calculates an adjusted interval dependent on the target frame time and actual frame time
                    int newInterval = targetMillisecondPerFrame - (int)elapsedTicks / TICKS_PER_MILLISECOND;
                    //Minimum timer tick is 1 millisecond
                    timer.Interval = newInterval > 1 ? newInterval : 1;

                    //Swaps value for next frame's calculations
                    lastIterationTime = start;
                    //Plus one complete frame
                    totalFrames++;

                    //Collect the garbage
                    clientDC.Dispose();
                    offScreenBmp.Dispose();
                    offScreenDC.Dispose();
                }
            };

            //Start the timer if running
            if (Running)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// Starts the timer that runs the animation.
        /// </summary>
        public void Start()
        {
            timer.Start();
            Running = true;
        }

        /// <summary>
        /// Stops the timer that runs the animation.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            Running = false;
        }

        /// <summary>
        /// Reinitializes the timing function of this class and restores the running state.
        /// </summary>
        public void Reset()
        {
            bool lastRunningState = Running;

            timer.Dispose();

            Initialize();

            if (lastRunningState)
            {
                Start();
            }
        }

        /// <summary>
        /// Calculates and returns a new frame of random static based off of effect settings.
        /// </summary>
        /// <returns>A Bitmap of random static based on the value of global varialbe: effects</returns>
        public Bitmap NextFrame()
        {
            //Calculate the number of pixels
            controlArea = control.Width * control.Height;
            //Prepare bitmap for return
            Bitmap bitmap = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppArgb);
            Rectangle lockRectangle = new Rectangle(0, 0, control.Width, control.Height);
            BitmapData bitmapData = bitmap.LockBits(lockRectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr intPointer = bitmapData.Scan0;

            byte[] pixels = new byte[controlArea * 4];

            //Prepare data arrays for storing random information
            byte[] redData = null;
            byte[] greenData = null;
            byte[] blueData = null;
            byte[] alphaData = null;
            byte[] lockData = null;
            byte[] absoluteData = null;
            byte[] invertData = null;

            //Tests for any locked flag (while that lock's pixel element is turned on)
            //If an element is locked we will need to assign random data to lockData
            if ((Effects & (Effect.Red | Effect.RedLock)) == (Effect.Red | Effect.RedLock) ||
                (Effects & (Effect.Green | Effect.GreenLock)) == (Effect.Green | Effect.GreenLock) ||
                (Effects & (Effect.Blue | Effect.BlueLock)) == (Effect.Blue | Effect.BlueLock) ||
                (Effects & (Effect.Alpha | Effect.AlphaLock)) == (Effect.Alpha | Effect.AlphaLock))
            {
                lockData = new byte[controlArea];

                //Do we want absolute values?
                if ((Effects & Effect.Absolute) == Effect.Absolute)
                {
                    //We want absolute values and will need random bits
                    //Create enough random bytes to extract the random bits we need
                    absoluteData = new byte[controlArea / 8 + 1];
                    random.NextBytes(absoluteData);

                    //Loop through all the bits to test for 1 or 0 and assign byte.MaxValue (255) if a 1
                    for (int index = 0; index < lockData.Length; index++)
                    {
                        //Bit shifts the number 1 left mod 8 of the index
                        //ANDs against a byte from absoluteData
                        //If the AND result is not 0, then the Nth (index mod 8) bit of the byte is a 1
                        //Essentially compares each bit in the array to 1
                        if ((absoluteData[index / 8] & (1 << index % 8)) != 0)
                        {
                            //Assign max value (255)
                            lockData[index] = byte.MaxValue;
                        }
                    }
                }
                else
                {
                    //We don't want absolute values and will need random bytes. This is easy.
                    random.NextBytes(lockData);
                }
            }

            //Is the Invert flag on? The Invert flag will make unasigned pixels values default to 255 instead of 0
            if ((Effects & Effect.Invert) == Effect.Invert || (Effects & Effect.Alpha) != Effect.Alpha)
            {
                invertData = new byte[controlArea];

                //Set all bytes to MaxValue (255)
                for (int index = 0; index < invertData.Length; index++)
                {
                    invertData[index] = byte.MaxValue;
                }
            }

            //////////////////////////////////
            //RED PIXEL COMPONENT CODE SEGMENT
            //////////////////////////////////

            //Is the red pixel turned on?
            if ((Effects & Effect.Red) == Effect.Red)
            {
                redData = new byte[controlArea];

                //Is the red pixel locked?
                if ((Effects & Effect.RedLock) == Effect.RedLock)
                {
                    //The red pixel is locked, use the lock data.
                    redData = lockData;
                }
                //Red pixel is not locked, do we want absolute values?
                else if ((Effects & Effect.Absolute) == Effect.Absolute)
                {
                    //We want absolute values and will need random bits
                    //Create enough random bytes to extract the random bits we need
                    absoluteData = new byte[controlArea / 8 + 1];
                    random.NextBytes(absoluteData);

                    //Loop through all the bits to test for 1 or 0 and assign byte.MaxValue (255) if a 1
                    for (int index = 0; index < redData.Length; index++)
                    {
                        //Bit shifts the number 1 left mod 8 of the index
                        //ANDs against a byte from absoluteData
                        //If the AND result is not 0, then the Nth (index mod 8) bit of the byte is a 1
                        //Essentially compares each bit in the array to 1
                        if ((absoluteData[index / 8] & (1 << index % 8)) != 0)
                        {
                            //Assign max value (255)
                            redData[index] = byte.MaxValue;
                        }
                    }
                }
                else
                {
                    //We don't want absolute values and will need random bytes. This is easy.
                    random.NextBytes(redData);
                }
            }
            //The red pixel is off, is Invert on?
            else if ((Effects & Effect.Invert) == Effect.Invert)
            {
                //All values will now be 255
                redData = invertData;
            }
            //The red pixel is off and Invert is off. redData will be zeroed.
            else
            {
                //All values will now be 0
                redData = new byte[controlArea];
            }

            ////////////////////////////////////
            //GREEN PIXEL COMPONENT CODE SEGMENT
            ////////////////////////////////////

            //This code section is structurally identical to the red pixel component
            if ((Effects & Effect.Green) == Effect.Green)
            {
                greenData = new byte[controlArea];

                if ((Effects & Effect.GreenLock) == Effect.GreenLock)
                {
                    greenData = lockData;
                }
                else if ((Effects & Effect.Absolute) == Effect.Absolute)
                {
                    absoluteData = new byte[controlArea / 8 + 1];
                    random.NextBytes(absoluteData);

                    for (int index = 0; index < greenData.Length; index++)
                    {
                        if ((absoluteData[index / 8] & (1 << index % 8)) != 0)
                        {
                            greenData[index] = byte.MaxValue;
                        }
                    }
                }
                else
                {
                    random.NextBytes(greenData);
                }
            }
            else if ((Effects & Effect.Invert) == Effect.Invert)
            {
                greenData = invertData;
            }
            else
            {
                greenData = new byte[controlArea];
            }

            ///////////////////////////////////
            //BLUE PIXEL COMPONENT CODE SEGMENT
            ///////////////////////////////////

            //This code section is structurally identical to the red pixel component
            if ((Effects & Effect.Blue) == Effect.Blue)
            {
                blueData = new byte[controlArea];

                if ((Effects & Effect.BlueLock) == Effect.BlueLock)
                {
                    blueData = lockData;
                }
                else if ((Effects & Effect.Absolute) == Effect.Absolute)
                {
                    absoluteData = new byte[controlArea / 8 + 1];
                    random.NextBytes(absoluteData);

                    for (int index = 0; index < blueData.Length; index++)
                    {
                        if ((absoluteData[index / 8] & (1 << index % 8)) != 0)
                        {
                            blueData[index] = byte.MaxValue;
                        }
                    }
                }
                else
                {
                    random.NextBytes(blueData);
                }
            }
            else if ((Effects & Effect.Invert) == Effect.Invert)
            {
                blueData = invertData;
            }
            else
            {
                blueData = new byte[controlArea];
            }

            ////////////////////////////////////
            //ALPHA PIXEL COMPONENT CODE SEGMENT
            ////////////////////////////////////

            //This code section is structurally identical to the red pixel component
            if ((Effects & Effect.Alpha) == Effect.Alpha)
            {
                alphaData = new byte[controlArea];

                if ((Effects & Effect.AlphaLock) == Effect.AlphaLock)
                {
                    alphaData = lockData;
                }
                else if ((Effects & Effect.Absolute) == Effect.Absolute)
                {
                    absoluteData = new byte[controlArea / 8 + 1];
                    random.NextBytes(absoluteData);

                    for (int index = 0; index < alphaData.Length; index++)
                    {
                        if ((absoluteData[index / 8] & (1 << index % 8)) != 0)
                        {
                            alphaData[index] = byte.MaxValue;
                        }
                    }
                }
                else
                {
                    random.NextBytes(alphaData);
                }
            }
            else
            {
                alphaData = invertData;
            }

            //Set our counters
            int pixelCount = 0;
            int dataCount = 0;

            //We will step through every pixel and assign the value from its respective array.
            //32bit Bitmap files are represented by four bytes, each corresponding to red, green, blue and alpha.
            //A single byte array represents this data in sets of four. At each cluster of four the first byte is blue,
            //the second byte is green, the third is red and the fourth is alpha.
            //Example:
            //pixels[0], pixels[4] and pixels [8] represent blue
            //pixels[1], pixels[5] and pixels [9] represent green
            //pixels[2], pixels[6] and pixels [10] represent red
            //pixels[3], pixels[7] and pixels [11] represent alpha
            //For visual simplicity, I stored the step values in an enum called Pixel
            while (pixelCount < pixels.Length)
            {
                //Assign the values to this group
                pixels[pixelCount + (int)Pixel.Red] = redData[dataCount];
                pixels[pixelCount + (int)Pixel.Green] = greenData[dataCount];
                pixels[pixelCount + (int)Pixel.Blue] = blueData[dataCount];
                pixels[pixelCount + (int)Pixel.Alpha] = alphaData[dataCount];

                //Step four spaces to the next group
                pixelCount += 4;
                //Step one space in the data arrays
                dataCount++;
            }

            /////////////////
            //TOP SECRET CODE
            /////////////////

            //This section does something cool and is top secret
            #region TopSecret
            if ((Effects & Effect.Secret) == Effect.Secret)
            {
                Bitmap secretBitmap = new Bitmap(control.Width, control.Height);
                string cypher = "Top Secret";
                byte[] partA = { 0x9C, 0xF6, 0xDC, 0x4C, 0xC2, 0xC7, 0x6D, 0xE0, 0xCA, 0xEF, 0xC9, 0x3, 0xE3, 0x45, 0x5D, 0xDE, 0xC8, 0xFC, 0xD9, 0xF1, 0xC6 };
                byte[] partB = { 0xA8, 0xF9, 0xD1, 0x4E, 0xBE, 0xBB, 0xDC, 0xFD, 0xDA, 0x99, 0x5E, 0xF7, 0xDF, 0x52, 0x73, 0xFD, 0xC8, 0xF7, 0xD3, 0xF3, 0x61, 0x9B, 0xD3, 0x55, 0xC5, 0x04, 0xD2, 0x03, 0xD8, 0xAD };
                byte[] part;


                Decrypt(partA, cypher);
                Decrypt(partB, cypher);

                special++;

                if (special / 100 % 2 == 0)
                {
                    part = partA;
                }
                else
                {
                    part = partB;
                }

                Graphics secretGraphics = Graphics.FromImage(secretBitmap);
                Font secretFont = new Font(FontFamily.GenericMonospace, 15f, FontStyle.Bold);
                int secretPadding = 20;

                SizeF currentSize;

                while (true)
                {
                    currentSize = secretGraphics.MeasureString(Encoding.Default.GetString(part), secretFont);

                    if (currentSize.Width + secretPadding <= control.Width && currentSize.Height + secretPadding <= control.Height)
                    {
                        secretFont = new Font(secretFont.FontFamily, secretFont.Size + 15, secretFont.Style);
                    }
                    else
                    {
                        break;
                    }
                }

                while (true)
                {
                    currentSize = secretGraphics.MeasureString(Encoding.Default.GetString(part), secretFont);

                    if (currentSize.Width + secretPadding * 2 > control.Width || currentSize.Height + secretPadding * 2 > control.Height)
                    {
                        secretFont = new Font(secretFont.FontFamily, secretFont.Size - 1, secretFont.Style);

                        if (secretFont.Size == 1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                int widthPadding = (int)(control.Width - currentSize.Width) / 2;
                int heightPadding = (int)(control.Height - currentSize.Height) / 2;

                secretGraphics.DrawString(Encoding.Default.GetString(part), secretFont, Brushes.White, new PointF(widthPadding, heightPadding));
                BitmapData secretBitmapData = secretBitmap.LockBits(lockRectangle, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                byte[] secretPixels = new byte[control.Width * control.Height * 4];
                IntPtr secretIntPointer = secretBitmapData.Scan0;
                Marshal.Copy(secretIntPointer, secretPixels, 0, secretPixels.Length);

                bool invert = (effect & Effect.Invert) == Effect.Invert;

                for (int index = 0; index < secretPixels.Length; index++)
                {
                    if (secretPixels[index] == 255)
                    {
                        if (index % 4 == (int)Pixel.Alpha)
                        {
                            pixels[index] = 255;
                        }
                        else if (index % 4 == (int)Pixel.Green && random.Next(0, 2) == 0)
                        {
                            pixels[index] = invert ? byte.MinValue : byte.MaxValue;
                        }
                    }
                }
            }
            #endregion

            //Is scanning flag on?
            if ((Effects & Effect.Scanning) == Effect.Scanning)
            {
                //The scanning flag is on
                int smear;

                //Calculate the start of the loop
                int loopStart = scanLocation * control.Width * 4;
                //Calculate the end of the loop
                int loopLimit = (scanLocation * control.Width + control.Width * ScanSize) * 4;
                loopLimit = loopLimit > pixels.Length ? pixels.Length : loopLimit;

                //Loop through all the pixels that we will apply the scan effect to
                for (int index = loopStart; index < loopLimit; index += 4)
                {
                    //Calculate smear, the size of the group of pixels we will smear together
                    smear = ScanSize - (index - loopStart) / (control.Width * 4);
                    smear = smear == 0 ? 1 : smear;
                    smear *= 2;

                    //Replace a pixel group of size smear with a pixel that is number smear away from the first pixel of the group
                    pixels[index + (int)Pixel.Red] = pixels[((index / (smear * 4) + 1) * smear * 4 + (int)Pixel.Red) % pixels.Length];
                    pixels[index + (int)Pixel.Green] = pixels[((index / (smear * 4) + 1) * smear * 4 + (int)Pixel.Green) % pixels.Length];
                    pixels[index + (int)Pixel.Blue] = pixels[((index / (smear * 4) + 1) * smear * 4 + (int)Pixel.Blue) % pixels.Length];
                    pixels[index + (int)Pixel.Alpha] = pixels[((index / (smear * 4) + 1) * smear * 4 + (int)Pixel.Alpha) % pixels.Length];
                }

                //Advances the vertical location of the scan. 5 seems to be a nice, smooth rate.
                scanLocation += 5;
                //Loop the scan
                scanLocation %= control.Height;
            }

            //Copys the data from pixels back into the bitmap
            Marshal.Copy(pixels, 0, intPointer, pixels.Length);

            //Unlocks the bitmap data
            bitmap.UnlockBits(bitmapData);

            //Return our frame
            return bitmap;
        }

        /// <summary>
        /// This function is used to hide text in secret messages
        /// </summary>
        /// <param name="data">Byte array to decrypt.</param>
        /// <param name="cypher">String to use for decryption.</param>
        public void Decrypt(byte[] data, string cypher)
        {
            for (int index = 0; index < data.Length; index++)
            {
                if (index % 2 == 0)
                {
                    data[index] -= (byte)cypher[index % cypher.Length];
                }
                else
                {
                    data[index] += (byte)cypher[index % cypher.Length];
                }
            }
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// Pass a byte array to paint the scan effect on. Must select a variant.
        /// </summary>
        /// <param name="pixels">A byte array to paint a scan effect on.</param>
        /// <param name="variant">The variant of scan effect to use. 1 through 4</param>
        [Obsolete("Present for historical purposes, compatible with current class model")]
        public void ScanEffectOld(byte[] pixels, int variant)
        {
            int divide;

            int loopStart = scanLocation * control.Width * 4;
            int loopLimit = (scanLocation * control.Width + control.Width * ScanSize) * 4;
            loopLimit = loopLimit > pixels.Length ? pixels.Length : loopLimit;

            for (int index = loopStart; index < loopLimit; index += 4)
            {

                divide = ScanSize - (index - loopStart) / ScanSize;
                divide = divide == 0 ? 1 : divide;

                switch (variant)
                {
                    case 1:
                        pixels[index + (int)Pixel.Red] = pixels[(index + divide + (index % divide) + (int)Pixel.Red) % pixels.Length];
                        pixels[index + (int)Pixel.Green] = pixels[(index + divide + (index % divide) + (int)Pixel.Green) % pixels.Length];
                        pixels[index + (int)Pixel.Blue] = pixels[(index + divide + (index % divide) + (int)Pixel.Blue) % pixels.Length];
                        pixels[index + (int)Pixel.Alpha] = pixels[(index + divide + (index % divide) + (int)Pixel.Alpha) % pixels.Length];
                        break;
                    case 2:
                        pixels[index + (int)Pixel.Red] = pixels[index / divide + (int)Pixel.Red];
                        pixels[index + (int)Pixel.Green] = pixels[index / divide + (int)Pixel.Green];
                        pixels[index + (int)Pixel.Blue] = pixels[index / divide + (int)Pixel.Blue];
                        pixels[index + (int)Pixel.Alpha] = pixels[index / divide + (int)Pixel.Alpha];
                        break;
                    case 3:
                        pixels[index + (int)Pixel.Red] = pixels[(index - (index % divide) + divide + (int)Pixel.Red) % pixels.Length];
                        pixels[index + (int)Pixel.Green] = pixels[(index - (index % divide) + divide + (int)Pixel.Green) % pixels.Length];
                        pixels[index + (int)Pixel.Blue] = pixels[(index - (index % divide) + divide + (int)Pixel.Blue) % pixels.Length];
                        pixels[index + (int)Pixel.Alpha] = pixels[(index - (index % divide) + divide + (int)Pixel.Alpha) % pixels.Length];
                        break;
                    case 4:
                        pixels[index + (int)Pixel.Red] = pixels[(index + divide * 4 - (index % divide) * 4 + (int)Pixel.Red) % pixels.Length];
                        pixels[index + (int)Pixel.Green] = pixels[(index + divide * 4 - (index % divide) * 4 + (int)Pixel.Green) % pixels.Length];
                        pixels[index + (int)Pixel.Blue] = pixels[(index + divide * 4 - (index % divide) * 4 + (int)Pixel.Blue) % pixels.Length];
                        pixels[index + (int)Pixel.Alpha] = pixels[(index + divide * 4 - (index % divide) * 4 + (int)Pixel.Alpha) % pixels.Length];
                        break;
                }
            }

            scanLocation += 5;
            scanLocation %= control.Height;
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// Pass a byte array to paint the scan effect on.
        /// </summary>
        /// <param name="pixels">A byte array to paint a scan effect on.</param>
        [Obsolete("Present for historical purposes, compatible with current class model")]
        public void ScanEffectOld2(byte[] pixels)
        {
            int location;
            int divide;

            //Scan location is global and marks the last vertical location that the scan bar was at
            //Based off of scan size will will step through that height of pixels and alter them
            for (int indexH = scanLocation; indexH < scanLocation + ScanSize && indexH < control.Height; indexH++)
            {
                //Step through every pixel horizontally in this line
                for (int indexW = 0; indexW < control.Width; indexW++)
                {
                    //Calculate our one dimensional location in the pixels array from our two demensional counting
                    location = (indexH * control.Width + indexW) * 4;
                    //Calculate a dividend to stretch pixels out. The higher we are, the longer the stretch
                    divide = ScanSize - indexH + scanLocation;
                    //Prevent divide by zero
                    divide = divide == 0 ? 1 : divide;//!!! must rethink this !!! //This is a note to myself to come back and redo this code

                    //Stretch pixels
                    pixels[location + (int)Pixel.Red] = pixels[location / divide + (int)Pixel.Red];
                    pixels[location + (int)Pixel.Green] = pixels[location / divide + (int)Pixel.Green];
                    pixels[location + (int)Pixel.Blue] = pixels[location / divide + (int)Pixel.Blue];
                    pixels[location + (int)Pixel.Alpha] = pixels[location / divide + (int)Pixel.Alpha];
                }
            }

            //5 lines each animation seems to be a plesant rate
            scanLocation += 5;
            //Wrap the scan location when it reaches the bottom
            scanLocation %= control.Height;
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// Replace "NextFrame()" in the method "Initialize()" to use. No other changes need to be made.
        /// This method was retired due to its inefficient process.
        /// </summary>
        /// <returns>A Bitmap of random static based on the value of global varialbe: effects</returns>
        [Obsolete("Present for historical purposes, compatible with Effects unum")]
        public Bitmap NextFrameOld()
        {
            double dataDensity = 0;
            bool anyLock = false;
            dataDensity -= (Effects & (Effect.Red | Effect.RedLock)) == (Effect.Red | Effect.RedLock) ? 1 : 0;
            dataDensity -= (Effects & (Effect.Green | Effect.GreenLock)) == (Effect.Green | Effect.GreenLock) ? 1 : 0;
            dataDensity -= (Effects & (Effect.Blue | Effect.BlueLock)) == (Effect.Blue | Effect.BlueLock) ? 1 : 0;
            dataDensity -= (Effects & (Effect.Alpha | Effect.AlphaLock)) == (Effect.Alpha | Effect.AlphaLock) ? 1 : 0;
            anyLock = dataDensity < 0;
            dataDensity += anyLock ? 1 : 0;
            dataDensity += (Effects & Effect.Red) == Effect.Red ? 1 : 0;
            dataDensity += (Effects & Effect.Green) == Effect.Green ? 1 : 0;
            dataDensity += (Effects & Effect.Blue) == Effect.Blue ? 1 : 0;
            dataDensity += (Effects & Effect.Alpha) == Effect.Alpha ? 1 : 0;
            dataDensity /= (Effects & Effect.Absolute) == Effect.Absolute ? 8 : 1;

            controlArea = control.Width * control.Height;
            Bitmap bitmap = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppArgb);
            Rectangle lockRectangle = new Rectangle(0, 0, control.Width, control.Height);
            BitmapData bitmapData = bitmap.LockBits(lockRectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr intPointer = bitmapData.Scan0;
            byte[] randomData;
            byte[] pixels = new byte[controlArea * 4];

            randomData = new byte[(int)(controlArea * dataDensity) + 1];

            random.NextBytes(randomData);

            int randomCount = 0;
            //int randomIncrement = dataDensity >= 1 ? (int)dataDensity : 1;
            int randomUseCount = 0;
            int pixelCount = 0;
            byte lockByte = byte.MinValue;

            while (pixelCount < pixels.Length)
            {
                if (anyLock)
                {
                    if ((Effects & Effect.Absolute) == Effect.Absolute)
                    {
                        if ((randomData[(randomCount + randomUseCount) / 8] & (1 << (randomCount + randomUseCount) % 8)) != 0)
                        {
                            lockByte = byte.MaxValue;
                        }
                        else
                        {
                            lockByte = byte.MinValue;
                        }
                    }
                    else
                    {
                        lockByte = randomData[randomCount + randomUseCount];
                    }

                    randomUseCount++;
                }

                if ((Effects & Effect.Red) == Effect.Red)
                {
                    if ((Effects & Effect.RedLock) == Effect.RedLock)
                    {
                        pixels[pixelCount + (int)Pixel.Red] = lockByte;
                    }
                    else if ((Effects & Effect.Absolute) == Effect.Absolute)
                    {
                        if ((randomData[(randomCount + randomUseCount) / 8] & (1 << (randomCount + randomUseCount) % 8)) != 0)
                        {
                            pixels[pixelCount + (int)Pixel.Red] = byte.MaxValue;
                        }

                        randomUseCount++;
                    }
                    else
                    {
                        pixels[pixelCount + (int)Pixel.Red] = randomData[randomCount + randomUseCount];
                        randomUseCount++;
                    }
                }
                else if ((Effects & Effect.Invert) == Effect.Invert)
                {
                    pixels[pixelCount + (int)Pixel.Red] = byte.MaxValue;
                }

                if ((Effects & Effect.Green) == Effect.Green)
                {
                    if ((Effects & Effect.GreenLock) == Effect.GreenLock)
                    {
                        pixels[pixelCount + (int)Pixel.Green] = lockByte;
                    }
                    else if ((Effects & Effect.Absolute) == Effect.Absolute)
                    {
                        if ((randomData[(randomCount + randomUseCount) / 8] & (1 << (randomCount + randomUseCount) % 8)) != 0)
                        {
                            pixels[pixelCount + (int)Pixel.Green] = byte.MaxValue;
                        }

                        randomUseCount++;
                    }
                    else
                    {
                        pixels[pixelCount + (int)Pixel.Green] = randomData[randomCount + randomUseCount];
                        randomUseCount++;
                    }
                }
                else if ((Effects & Effect.Invert) == Effect.Invert)
                {
                    pixels[pixelCount + (int)Pixel.Green] = byte.MaxValue;
                }

                if ((Effects & Effect.Blue) == Effect.Blue)
                {
                    if ((Effects & Effect.BlueLock) == Effect.BlueLock)
                    {
                        pixels[pixelCount + (int)Pixel.Blue] = lockByte;
                    }
                    else if ((Effects & Effect.Absolute) == Effect.Absolute)
                    {
                        if ((randomData[(randomCount + randomUseCount) / 8] & (1 << (randomCount + randomUseCount) % 8)) != 0)
                        {
                            pixels[pixelCount + (int)Pixel.Blue] = byte.MaxValue;
                        }

                        randomUseCount++;
                    }
                    else
                    {
                        pixels[pixelCount + (int)Pixel.Blue] = randomData[randomCount + randomUseCount];
                        randomUseCount++;
                    }
                }
                else if ((Effects & Effect.Invert) == Effect.Invert)
                {
                    pixels[pixelCount + (int)Pixel.Blue] = byte.MaxValue;
                }

                if ((Effects & Effect.Alpha) == Effect.Alpha)
                {
                    if ((Effects & Effect.AlphaLock) == Effect.AlphaLock)
                    {
                        pixels[pixelCount + (int)Pixel.Alpha] = lockByte;
                    }
                    else if ((Effects & Effect.Absolute) == Effect.Absolute)
                    {
                        if ((randomData[(randomCount + randomUseCount) / 8] & (1 << (randomCount + randomUseCount) % 8)) != 0)
                        {
                            pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;
                        }

                        randomUseCount++;
                    }
                    else
                    {
                        pixels[pixelCount + (int)Pixel.Alpha] = randomData[randomCount + randomUseCount];
                        randomUseCount++;
                    }
                }
                else
                {
                    pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;
                }

                //randomCount += randomIncrement;
                randomCount += randomUseCount;
                randomUseCount = 0;
                pixelCount += 4;
            }

            Marshal.Copy(pixels, 0, intPointer, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// Replace "NextFrame()" in the method "Initialize()" to use. No other changes need to be made.
        /// This method was retired due to its lack of creativity.
        /// </summary>
        /// <returns>A Bitmap of random static based on the value of global varialbe: format</returns>
        [Obsolete("Present for historical purposes, use Format enum for compatability with this method")]
        public Bitmap NextFrameOld2()
        {
            controlArea = control.Width * control.Height;
            Bitmap bitmap = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppArgb);
            Rectangle lockRectangle = new Rectangle(0, 0, control.Width, control.Height);
            BitmapData bitmapData = bitmap.LockBits(lockRectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr intPointer = bitmapData.Scan0;
            byte[] randomData;
            byte[] pixels = new byte[controlArea * 4];

            if (format == Format.BlackAndWhite)
            {
                randomData = new byte[controlArea / 8 + 1];
                random.NextBytes(randomData);

                int randomCount = 0;
                int pixelCount = 0;

                while (pixelCount < pixels.Length)
                {
                    if ((randomData[randomCount / 8] & (1 << randomCount % 8)) != 0) //mod index to get neat results in random data
                    {
                        pixels[pixelCount + (int)Pixel.Red] = byte.MaxValue;
                        pixels[pixelCount + (int)Pixel.Green] = byte.MaxValue;
                        pixels[pixelCount + (int)Pixel.Blue] = byte.MaxValue;
                    }

                    pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                    randomCount++;
                    pixelCount += 4;
                }
            }
            else if (true)//format == Format.GrayScale)
            {
                randomData = new byte[controlArea];
                random.NextBytes(randomData);

                int randomCount = 0;
                int pixelCount = 0;
                byte shade;

                while (pixelCount < pixels.Length)
                {
                    shade = randomData[randomCount];

                    pixels[pixelCount + (int)Pixel.Red] = shade;
                    pixels[pixelCount + (int)Pixel.Green] = shade;
                    pixels[pixelCount + (int)Pixel.Blue] = shade;

                    pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                    randomCount++;
                    pixelCount += 4;
                }
            }
#pragma warning disable CS0162
            else if (false)//grayscale with a twist
#pragma warning restore CS0162
            {
                randomData = new byte[controlArea / 8 + 1];// / 8 + 1];
                random.NextBytes(randomData);

                int count = 0;

                for (int index = 0; index < controlArea; index++)
                {
                    count = index * 4;
                    byte shade = randomData[index / 8];

                    pixels[count + (int)Pixel.Red] = shade;
                    pixels[count + (int)Pixel.Green] = shade;
                    pixels[count + (int)Pixel.Blue] = shade;

                    pixels[count + (int)Pixel.Alpha] = byte.MaxValue;
                }
            }
            else if (format == Format.ColorAndAlpha)
            {
                randomData = new byte[controlArea * 3];
                random.NextBytes(randomData);

                int randomCount = 0;
                int pixelCount = 0;

                while (pixelCount < pixels.Length)
                {
                    pixels[pixelCount + (int)Pixel.Red] = randomData[randomCount + (int)Pixel.Red];
                    pixels[pixelCount + (int)Pixel.Green] = randomData[randomCount + (int)Pixel.Green];
                    pixels[pixelCount + (int)Pixel.Blue] = randomData[randomCount + (int)Pixel.Blue];

                    pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                    randomCount += 3;
                    pixelCount += 4;
                }
            }



            Marshal.Copy(pixels, 0, intPointer, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// This is the old StaticGenerator class and is kept for historical purposes.
        /// Commenting is not present.
        /// </summary>
        [Obsolete("Present for historical purposes, do not use. Not compatible with current class model")]
        public class Old
        {
            private Random random;
            private Format format;
            public int Width { get; private set; }
            public int Height { get; private set; } //transition to public set
            public int Size
            {
                get
                {
                    return Width * Height;
                }
            }
            public bool Blurring { get; set; }
            private int resolution;

            [Flags]
            public enum Format
            {
                BlackAndWhite = 0x0,
                GrayScale = 0x1,
                Color = 0x2,
                ColorAndAlpha = 0x4
            }

            public enum Pixel
            {
                Red = 0x0,
                Green = 0x1,
                Blue = 0x2,
                Alpha = 0x3
            }


            public int Resolution
            {
                get
                {
                    return resolution;
                }
                set
                {
                    if (value >= 1)
                    {
                        resolution = value;
                    }
                    else
                    {
                        throw new Exception("Resolution must be equal to or greater than one.");
                    }
                }
            }

            public Old(int width, int height, Format format, int resolution, bool blurring)//Depth depth, Format format, int resolution, bool blurring)
            {
                //bitmap = new Bitmap(width, height);
                random = new Random((int)DateTime.Now.Ticks);
                Width = width;
                Height = height;
                Resolution = resolution;
                Blurring = blurring;
                //this.depth = depth;
                this.format = format;

            }

            public Bitmap NextFrame()
            {
                Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Rectangle lockRectangle = new Rectangle(0, 0, Width, Height);
                BitmapData bitmapData = bitmap.LockBits(lockRectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                IntPtr intPointer = bitmapData.Scan0;
                byte[] randomData;
                byte[] pixels = new byte[Size * 4];

                if (format == Format.BlackAndWhite)
                {
                    randomData = new byte[Size / 8 + 1];
                    random.NextBytes(randomData);

                    int randomCount = 0;
                    int pixelCount = 0;

                    while (pixelCount < pixels.Length)
                    {
                        if ((randomData[randomCount / 8] & (1 << randomCount % 8)) != 0) //mod index to get neat results in random data
                        {
                            pixels[pixelCount + (int)Pixel.Red] = byte.MaxValue;
                            pixels[pixelCount + (int)Pixel.Green] = byte.MaxValue;
                            pixels[pixelCount + (int)Pixel.Blue] = byte.MaxValue;
                        }

                        pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                        randomCount++;
                        pixelCount += 4;
                    }
                }
                else if (true)//format == Format.GrayScale)
                {
                    randomData = new byte[Size];
                    random.NextBytes(randomData);

                    int randomCount = 0;
                    int pixelCount = 0;
                    byte shade;

                    while (pixelCount < pixels.Length)
                    {
                        shade = randomData[randomCount];

                        pixels[pixelCount + (int)Pixel.Red] = shade;
                        pixels[pixelCount + (int)Pixel.Green] = shade;
                        pixels[pixelCount + (int)Pixel.Blue] = shade;

                        pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                        randomCount++;
                        pixelCount += 4;
                    }
                }
#pragma warning disable CS0162
                else if (false)//grayscale with a twist
#pragma warning restore CS0162
                {
                    randomData = new byte[Size / 8 + 1];// / 8 + 1];
                    random.NextBytes(randomData);

                    int count = 0;
                    int limit = Size;

                    for (int index = 0; index < limit; index++)
                    {
                        count = index * 4;
                        byte shade = randomData[index / 8];

                        pixels[count + (int)Pixel.Red] = shade;
                        pixels[count + (int)Pixel.Green] = shade;
                        pixels[count + (int)Pixel.Blue] = shade;

                        pixels[count + (int)Pixel.Alpha] = byte.MaxValue;
                    }
                }
                else if (format == Format.ColorAndAlpha)
                {
                    randomData = new byte[Size * 3];
                    random.NextBytes(randomData);

                    int randomCount = 0;
                    int pixelCount = 0;

                    while (pixelCount < pixels.Length)
                    {
                        pixels[pixelCount + (int)Pixel.Red] = randomData[randomCount + (int)Pixel.Red];
                        pixels[pixelCount + (int)Pixel.Green] = randomData[randomCount + (int)Pixel.Green];
                        pixels[pixelCount + (int)Pixel.Blue] = randomData[randomCount + (int)Pixel.Blue];

                        pixels[pixelCount + (int)Pixel.Alpha] = byte.MaxValue;

                        randomCount += 3;
                        pixelCount += 4;
                    }
                }



                Marshal.Copy(pixels, 0, intPointer, pixels.Length);

                bitmap.UnlockBits(bitmapData);

                return bitmap;
            }

            [Obsolete("Present for historical purposes, do not use. Not compatible with current class model", true)]
            private void NextFrameOld()
            {
                /*
                //OLD
                else if (format == Format.ColorAndAlpha)
                {
                    randomData = new byte[Size * 3];// / 8 + 1];
                    random.NextBytes(randomData);

                    int count = 0;
                    int limit = Size;//randomData.Length

                    for (int index = 0; index < limit; index++)
                    {
                        count += 4;

                        pixels[count + (int)Pixel.Red] = randomData[index + (int)Pixel.Red];
                        pixels[count + (int)Pixel.Green] = randomData[index + (int)Pixel.Green];
                        pixels[count + (int)Pixel.Blue] = randomData[index + (int)Pixel.Blue];

                        pixels[count + (int)Pixel.Alpha] = byte.MaxValue;
                    }
                }

                else if (format == Format.GrayScale)
                {
                    randomData = new byte[Size];// / 8 + 1];
                    random.NextBytes(randomData);

                    int count = 0;

                    for (int index = 0; index < randomData.Length; index++)
                    {
                        count = index * 4;
                        byte shade = randomData[index];
                    
                        pixels[count + (int)Pixel.Red] = shade;
                        pixels[count + (int)Pixel.Green] = shade;
                        pixels[count + (int)Pixel.Blue] = shade;

                        pixels[count + (int)Pixel.Alpha] = byte.MaxValue;
                    }
                }

                if (format == Format.BlackAndWhite)
                {
                    randomData = new byte[Size / 8 + 1];
                    random.NextBytes(randomData);

                    int count = 0;
                    int limit = Size;

                    for (int index = 0; index < limit; index++)
                    {
                        count = index * 4;
                        if ((randomData[index / 8] & (1 << index % 8)) != 0) //mod index to get neat results in random data
                        {
                            pixels[count + (int)Pixel.Red] = byte.MaxValue;
                            pixels[count + (int)Pixel.Green] = byte.MaxValue;
                            pixels[count + (int)Pixel.Blue] = byte.MaxValue;
                        }

                        pixels[count + (int)Pixel.Alpha] = byte.MaxValue;
                    }
                }
                */
            }
        }
    }
}
