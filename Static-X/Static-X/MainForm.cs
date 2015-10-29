using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Static_X
{
    public partial class MainForm : Form
    {
        private Timer timer;
        private DateTime lastTime;
        private StaticGenerator staticGenerator;
        private CheckBox secretCheckBox = new CheckBox();
        private bool discovered;
        private byte exclusive;
        private byte inclusive;

        /// <summary>
        /// Gets the state of the program
        /// </summary>
        public bool Running
        {
            get
            {
                return startCheckBox.Checked;
            }
        }

        /// <summary>
        /// Call all initializations
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeGenerator();
            Incubate();
        }

        /// <summary>
        /// Initialize the static generator
        /// </summary>
        private void InitializeGenerator()
        {
            staticGenerator = new StaticGenerator(staticPictureBox);
            staticGenerator.Start();
        }

        /// <summary>
        /// Starts and stops the generator
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void startCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!Running)
            {
                staticGenerator.Stop();
                startCheckBox.Text = "Start";
            }
            else
            {
                staticGenerator.Start();
                startCheckBox.Text = "Stop";
            }
        }

        /// <summary>
        /// Controls the target frames per second value
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void fpsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            staticGenerator.FramesPerSecond = (int)fpsNumericUpDown.Value;
        }

        /// <summary>
        /// Shows or hides the generator details
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void detailsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            staticGenerator.ShowDetails = detailsCheckBox.Checked;
        }

        /// <summary>
        /// Changes any variety of generator effects
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void modifierCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            StaticGenerator.Effect effects = StaticGenerator.Effect.None;

            effects |= redCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Red : StaticGenerator.Effect.None;
            effects |= redCheckBox.CheckState == CheckState.Indeterminate ? StaticGenerator.Effect.Red | StaticGenerator.Effect.RedLock : StaticGenerator.Effect.None;
            effects |= greenCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Green : StaticGenerator.Effect.None;
            effects |= greenCheckBox.CheckState == CheckState.Indeterminate ? StaticGenerator.Effect.Green | StaticGenerator.Effect.GreenLock : StaticGenerator.Effect.None;
            effects |= blueCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Blue : StaticGenerator.Effect.None;
            effects |= blueCheckBox.CheckState == CheckState.Indeterminate ? StaticGenerator.Effect.Blue | StaticGenerator.Effect.BlueLock : StaticGenerator.Effect.None;
            effects |= alphaCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Alpha : StaticGenerator.Effect.None;
            effects |= alphaCheckBox.CheckState == CheckState.Indeterminate ? StaticGenerator.Effect.Alpha | StaticGenerator.Effect.AlphaLock : StaticGenerator.Effect.None;
            effects |= absoluteCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Absolute : StaticGenerator.Effect.None;
            effects |= scanningCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Scanning : StaticGenerator.Effect.None;
            effects |= invertCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Invert : StaticGenerator.Effect.None;
            effects |= secretCheckBox.CheckState == CheckState.Checked ? StaticGenerator.Effect.Secret : StaticGenerator.Effect.None;

            staticGenerator.Effects = effects;

            Curiosity(effects);
        }

        /// <summary>
        /// Resets the generator class
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            staticGenerator.Reset();
        }

        /// <summary>
        /// Secret Code, does cool stuff
        /// </summary>
        private void Incubate()
        {
            string cypher = "Top Secret";
            byte[] text = { 0x99, 0xF2, 0xE3, 0x54, 0xB8, 0x0D, 0x83, 0xD3, 0xCC, 0xF3 };
            staticGenerator.Decrypt(text, cypher);

            secretCheckBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            secretCheckBox.AutoSize = true;
            secretCheckBox.Location = new Point(219, 376);
            secretCheckBox.Name = "secretCheckBox";
            secretCheckBox.Size = new Size(57, 17);
            secretCheckBox.TabIndex = 13;
            secretCheckBox.Text = Encoding.Default.GetString(text);
            secretCheckBox.UseVisualStyleBackColor = true;
            secretCheckBox.CheckStateChanged += new EventHandler(this.modifierCheckBox_CheckStateChanged);
            secretCheckBox.Visible = false;
            Controls.Add(secretCheckBox);

            Timer timer = new Timer();
            timer.Interval = 60000;
            timer.Tick += (object sender, EventArgs e) =>
            {
                timer.Stop();

                if (!discovered)
                {
                    byte[] messageA = { 0x98, 0x00, 0x90, 0x59, 0xC2, 0x10, 0x83, 0xFA, 0xCE, 0xF7, 0xB9, 0xB1, 0xB5, 0x41, 0xC6, 0x0F, 0xC8, 0x00, 0x85, 0xD1, 0xBB, 0xF8, 0xE3, 0x1F };
                    byte[] headingA = { 0x9C, 0xF2, 0xE4, 0x43, 0xBB, 0x04, 0xD1, 0xF5 };
                    byte[] messageB = { 0xA7, 0xF6, 0xE2, 0x49, 0xC2, 0x10, 0xD6, 0xFA, 0xDE, 0xCB, 0x74, 0xE8, 0xD8, 0x4F, 0x73, 0xFF, 0xD2, 0xF3, 0xD8, 0xFA, 0x7B, 0x05, 0x90, 0x4C, 0xBC, 0x06, 0xC8, 0xAE, 0xAA, 0xED, 0xC7, 0x05, 0xD5, 0x52, 0x73, 0xE0, 0xCA, 0xF5, 0xD8, 0xCB, 0x74, 0xE1, 0xDC, 0x45, 0xB4, 0x0E, 0xC8, 0xAE, 0xC8, 0xF8, 0xBD, 0xF4, 0xDB, 0x00, 0xA2, 0xE6, 0x8F, 0xAE, 0xAE, 0xAC, 0xC1, 0xF2, 0xD4, 0x45, 0x73, 0x0F, 0xCB, 0xF7, 0xD8, 0xAC, 0xBA, 0x00, 0xE2, 0x00, 0xCC, 0x0A, 0xD8, 0xBC };
                    byte[] headingB = { 0xA8, 0x03, 0xE9, 0x00, 0x94, 0x02, 0xC4, 0xF7, 0xD3 };
                    staticGenerator.Decrypt(messageA, cypher);
                    staticGenerator.Decrypt(headingA, cypher);
                    staticGenerator.Decrypt(messageB, cypher);
                    staticGenerator.Decrypt(headingB, cypher);

                    switch (MessageBox.Show(Encoding.Default.GetString(messageA), Encoding.Default.GetString(headingA), MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            break;
                        case DialogResult.No:
                        case DialogResult.Cancel:
                            MessageBox.Show(Encoding.Default.GetString(messageB), Encoding.Default.GetString(headingB), MessageBoxButtons.OK);
                            break;
                    }

                    Hatch();
                }
            };
            timer.Start();
        }

        /// <summary>
        /// Secret Code, does cool stuff
        /// </summary>
        private void Hatch()
        {
            if (!discovered)
            {
                redCheckBox.CheckState = CheckState.Indeterminate;
                greenCheckBox.CheckState = CheckState.Indeterminate;
                blueCheckBox.CheckState = CheckState.Indeterminate;
                alphaCheckBox.CheckState = CheckState.Checked;
                absoluteCheckBox.CheckState = CheckState.Unchecked;
                scanningCheckBox.CheckState = CheckState.Checked;
                invertCheckBox.CheckState = CheckState.Unchecked;
                secretCheckBox.CheckState = CheckState.Checked;
                secretCheckBox.Visible = true;
                discovered = true;
            }
        }

        /// <summary>
        /// Secret Code, does cool stuff
        /// </summary>
        private void Curiosity(StaticGenerator.Effect effects)
        {
            inclusive |= (byte)effects;
            exclusive ^= (byte)effects;

            if (inclusive == 255 && exclusive < 100 || exclusive > inclusive)
            {
                Hatch();
            }
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// Replace "InitializeGenerator()" in the method "MainForm()" to use. No other changes need to be made.
        /// This method was retired with its class.
        /// </summary>
        [Obsolete("Present for historical purposes, do not use. Not compatible with current class model")]
        private void OLD_InitializeStaticGenerator()
        {
            StaticGenerator.Old staticGenerator = new StaticGenerator.Old(staticPictureBox.Width, staticPictureBox.Height, StaticGenerator.Old.Format.ColorAndAlpha, 1, false);//StaticGenerator.Depth._32Bit, StaticGenerator.Format.ColorAndAlpha);
            lastTime = DateTime.Now;

            double totalFrameRate = 0;
            int count = 1;
            double frameRate = 0;
            double averageFrameRate = 0;
            int fps = (int)fpsNumericUpDown.Value;


            timer = new Timer();
            timer.Interval = 1000 / fps;
            timer.Tick += (object sender, EventArgs e) =>
            {
                Bitmap offScreenBmp;
                Graphics offScreenDC;
                offScreenBmp = new Bitmap(staticGenerator.Width, staticGenerator.Height);
                offScreenDC = Graphics.FromImage(offScreenBmp);
                Graphics clientDC = staticPictureBox.CreateGraphics();

                DateTime start = DateTime.Now;
                double frameTicks = start.Subtract(lastTime).Ticks;

                offScreenDC.DrawImage(staticGenerator.NextFrame(), new Point(0, 0));

                if (detailsCheckBox.Checked)
                {
                    Rectangle backRectangle = new Rectangle(new Point(0, 0), new Size(250, 32));
                    offScreenDC.FillRectangle(new SolidBrush(Color.White), backRectangle);
                    string screenText = "FPS: ";
                    frameRate = 10000000 / frameTicks;
                    totalFrameRate += frameRate;
                    averageFrameRate = totalFrameRate / count;
                    count++;
                    screenText += frameRate + "\nAverage FPS: " + averageFrameRate;

                    offScreenDC.DrawString(screenText, new Font(FontFamily.GenericMonospace, 10f, FontStyle.Bold), Brushes.Black, new PointF(0f, 0f));

                }

                clientDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                clientDC.DrawImage(offScreenBmp, 0, 0);

                DateTime end = DateTime.Now;

                int mpf = 1000 / fps;
                double elapsedTicks = end.Subtract(start).Ticks;
                int b = (int)elapsedTicks / 10000;
                int newInterval = mpf - b;
                timer.Interval = newInterval > 1 ? newInterval : 1;

                lastTime = start;
            };
            timer.Start();

            if (!Running)
            {
                System.Threading.Thread.Sleep(timer.Interval);
                timer.Stop();
            }
        }

        /// <summary>
        /// This method is kept for historical purposes and not implemented in this current class model.
        /// This method was retired with its class.
        /// </summary>
        [Obsolete("Present for historical purposes, do not use. Not compatible with current class model")]
        private void OLD_DisposeStaticGenerator()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
