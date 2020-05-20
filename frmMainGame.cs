using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MINISwepper2
{
    public enum eGameMode
    {
        NewGame = 0,
        BeginnerMode = 1,
        MediumMode = 2,
        ExpertMode = 3,
        Exit = 4
    }

    public partial class frmMainGame : Form
    {


        private const int c_BtnWidth = 40;
        private const int c_BtnHeight = 40;
        private const int c_RowColumnAmount = 10;
        private const int c_Padding = 40;
        private const string c_FlagImageName = "flag.png";
        private const string c_MonkeyFailImageName = "monkeyFail.png";
        private const string c_MonkeySmileImageName = "monkeySmile.png";
        private const string c_BoombImageName = "boomb.jpeg";
        private const string c_ImageFolderName = "images";
        private const int c_IconWidth = 70;
        private const int c_IconHeight = 60;
        private const int c_TopLeftY = 200;
        private const string c_BoomText = "X";

        private ComboBox m_ComboBox = new ComboBox();

        private Button[,] btnMatrix;
        private PictureBox monkeySmile = new PictureBox();
        private PictureBox monkeyFail = new PictureBox();
        private int m_NumOfBoombs = 5;
        private string m_NamePlayer;
        private string m_ModeGame;
        private Label lblName = new Label();
        private Button btnStartGame, btnStopGame;
        private Label lblTimer;
        private ComboBox ComboBox1;
        private DateTime m_StartGameTime;
        private DateTime m_StopGameTime;
        private TimeSpan m_TotalBreakTime;
        private bool m_IsFirstClick = true;
        private ScoreManager m_ScoreManager;
        public frmMainGame()
        {
            InitializeComponent();
            InitPlayerName();
            InitTimer();
            this.Width = c_BtnWidth * c_RowColumnAmount + c_Padding;
            this.Height = c_BtnHeight * c_RowColumnAmount + c_Padding + c_TopLeftY;
            btnMatrix = new Button[c_RowColumnAmount, c_RowColumnAmount];
            m_ScoreManager = new ScoreManager();
            InitInitializeComboBox();
            InitImage();
            InitBoardGame();
            StartGame(m_NumOfBoombs);
        }

        private void InitTimer()
        {
            lblTimer = new Label();
            lblTimer.Text = "Time:" + "00:00:00";
            lblTimer.Location = new Point(140, 100);
            lblTimer.Size = new Size(125, 25);

            btnStartGame = new Button();
            btnStartGame.Text = "New Game";
            btnStartGame.Location = new Point(25, 100);

            btnStopGame = new Button();
            btnStopGame.Text = "Pause Game";
            btnStopGame.Location = new Point(this.Width - 125, 100);

            btnStartGame.Size = btnStopGame.Size = new Size(100, 100);
            btnStopGame.Click += BtnStopGame_Click;
            btnStartGame.Click += BtnStartGame_Click;

            this.Controls.Add(lblTimer);
            this.Controls.Add(btnStartGame);
            this.Controls.Add(btnStopGame);

            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000;
            m_TotalBreakTime = default;




        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.BackColor = System.Drawing.Color.BlanchedAlmond;

            lblTimer.Text = "Time: " + (DateTime.Now - m_StartGameTime).Subtract(m_TotalBreakTime).ToString(@"hh\:mm\:ss");
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {

                timer1.Start();
                m_StartGameTime = DateTime.Now;
                m_TotalBreakTime = default;
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want to play New Board?", "New Game", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) StartGame(m_NumOfBoombs);

            }


        }

        private void BtnStopGame_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled)
            {
                timer1.Stop();
                m_StopGameTime = DateTime.Now;
                btnStopGame.BackColor = Color.Red;
            }
            else
            {
                timer1.Start();
                m_TotalBreakTime += DateTime.Now.Subtract(m_StopGameTime);
                btnStopGame.BackColor = default;
            }

        }

        private void InitPlayerName()
        {
            frmSetName setName = new frmSetName();
            setName.ShowDialog();
            m_NamePlayer = setName.Name;
            lblName.Text = "Player: " + setName.Name;
            lblName.BackColor = Color.Aqua;
            lblName.Location = new Point(this.Width - 150, 50);
            lblName.Font = new Font("ariel", 12);
            this.Controls.Add(lblName);
        }

        private void StartGame(int c_ModeGame)
        {
            m_NumOfBoombs = c_ModeGame;
            CleanBoard();
            InitBombs();
            InitNumbers();
            btnStopGame.BackColor = Color.LightGray;
            m_StartGameTime = DateTime.Now;
            m_StopGameTime = default;
            m_TotalBreakTime = default;
            monkeyFail.Visible = false;
            monkeySmile.Visible = true;
            m_IsFirstClick = true;
        }

        private bool IsWinGame()
        {
            int boombCnt = 0;
            foreach (var btn in btnMatrix)
            {
                // check if all flag are on boom
                if (IsFlag(btn) && !IsBtnBoom(btn))
                    return false;

                // check if btn is not flag and not boomb (regular btn) and not revealed yet
                if (!IsFlag(btn) && !IsBtnBoom(btn) && string.IsNullOrEmpty(btn.Text))
                    return false;

                // count all flag
                if (IsFlag(btn))
                    boombCnt++;
            }
            return boombCnt == m_NumOfBoombs;
        }


        /* Initialize Number to Button if Button==Boomb pass */
        private void InitNumbers()
        {


            for (int i = 0; i < btnMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < btnMatrix.GetLength(1); j++)
                {
                    if (IsBtnBoom(i, j))
                        continue;

                    btnMatrix[i, j].Tag = GetBombsAmount(i, j).ToString();

                }

            }
        }
        /* this funcation return to button how much  boomb have round */
        private int GetBombsAmount(int row, int col)
        {
            int countBoom = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (CheckValidBtn(i, j) && IsBtnBoom(i, j))
                        countBoom++;

                }
            }
            return countBoom;
        }
        /* */
        public bool CheckValidBtn(int x, int y)
        {
            return (x >= 0 && x < this.btnMatrix.GetLength(0) &&
                    y >= 0 && y < this.btnMatrix.GetLength(1));
        }

        /* This Func clean all attribute of Button*/

        public void CleanBoard()
        {
            for (int i = 0; i < btnMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < btnMatrix.GetLength(1); j++)
                {
                    btnMatrix[i, j].Tag = 0;
                    btnMatrix[i, j].Enabled = true;
                    btnMatrix[i, j].Image = null;
                    btnMatrix[i, j].Text = "";
                }

            }
        }
        /* Set randmom button to Matrix*/
        private void InitBombs()
        {
            Random random = new Random();
            int row = 0;
            int col = 0;

            for (int i = 0; i < m_NumOfBoombs; i++)
            {
                do
                {
                    row = random.Next(0, 9);
                    col = random.Next(0, 9);

                } while (IsBtnBoom(row, col));

                btnMatrix[row, col].Tag = c_BoomText;
            }


        }

        private bool IsBtnBoom(int row, int col)
        {

            return IsBtnBoom(btnMatrix[row, col]);

        }
        private bool IsBtnBoom(Button btn)
        {
            return btn.Tag != null &&
            btn.Tag.ToString().CompareTo(c_BoomText) == 0;
        }
        /* Initialize Board this create button  on the board and call to Mouse Function*/
        private void InitBoardGame()
        {
            for (int i = 0; i < btnMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < btnMatrix.GetLength(1); j++)
                {


                    btnMatrix[i, j] = new Button();
                    btnMatrix[i, j].Location = new Point(i * c_BtnWidth, j * c_BtnHeight + c_TopLeftY);
                    btnMatrix[i, j].Width = c_BtnWidth;
                    btnMatrix[i, j].Height = c_BtnHeight;
                    btnMatrix[i, j].BackColor = Color.LightBlue;
                    btnMatrix[i, j].Font = new Font("French Script MT", 18);
                    this.Controls.Add(btnMatrix[i, j]);
                    // Set event on Mousedown
                    btnMatrix[i, j].MouseDown += OnMouseDown;

                }

            }
        }

        /// <summary>
        /// Function on Mouse Down Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            if (m_StartGameTime == default)
                BtnStartGame_Click(sender, e);

            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
                OnLeftMouseClick(btn);
            else if (e.Button == MouseButtons.Right)
                OnRightMouseClick(btn);

            if (IsWinGame())
            {
                PlayerScore ps = new PlayerScore();
                ps.Name = m_NamePlayer;
                ps.Mode = (eGameMode)(m_ComboBox.SelectedIndex);
                m_ScoreManager.SetScore(ps, true);
                MessageBox.Show("You are a Winner GEVER");

            }


        }

        private void OnLeftMouseClick(Button clickedBtn)
        {

            if (m_IsFirstClick && IsBtnBoom(clickedBtn))
            {
                StartGame(m_NumOfBoombs);

            }
            m_IsFirstClick = false;

            if (IsFlag(clickedBtn))
                return;

            if (IsBtnBoom(clickedBtn))
            {
                clickedBtn.Image = InsertImageToButton(c_BoombImageName);
                GameOver();
                return;
            }

            if (IsZero(clickedBtn))
            {

                if (GetRowColOfBtn(clickedBtn, out int row, out int col))
                    DislayAllZeros(clickedBtn, row, col);
            }

            DisplayBtnText(clickedBtn);
        }
        private void DisplayBtnText(Button btn)
        {
            // Puts number to btn
            btn.Text = btn.Tag.ToString();

            // disable to click again on that btn
            btn.Enabled = false;
        }
        private void DislayAllZeros(Button btn, int row, int col)
        {

            if (!IsZero(btn) || !CheckValidBtn(row, col) || !btn.Enabled)
                return;

            DisplayBtnText(btn);

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (CheckValidBtn(i, j))
                        DislayAllZeros(btnMatrix[i, j], i, j);
                }
            }
        }

        private bool GetRowColOfBtn(Button btn, out int row, out int col)
        {
            for (int i = 0; i < btnMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < btnMatrix.GetLength(1); j++)
                {
                    if (btnMatrix[i, j] == btn)
                    {
                        row = i;
                        col = j;
                        return true;

                    }
                }

            }
            row = 0;
            col = 0;
            return false;

        }


        private bool IsZero(Button btn)
        {
            return btn.Tag != null &&
                 int.Parse(btn.Tag.ToString()) == 0;
        }

        private bool IsFlag(Button clickBtn)
        {
            return clickBtn.Image != null;
        }

        private void GameOver()
        {
            PlayerScore ps = new PlayerScore();
            ps.Name = m_NamePlayer;
            ps.Mode = (eGameMode)(m_ComboBox.SelectedIndex);
            monkeyFail.Visible = true;
            monkeySmile.Visible = false;
            m_ScoreManager.SetScore(ps,false);
            DialogResult result = MessageBox.Show("Do you want to play again?", "Game Over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) StartGame(m_NumOfBoombs);
            else
            {
                this.Close();
            }
        }

        private void OnRightMouseClick(Button clickBtn)
        {


            InsertFlag(clickBtn);


        }

        private void InsertFlag(Button clickBtn)
        {
            if (clickBtn.Image == null)
            {
                clickBtn.Image = InsertImageToButton(c_FlagImageName);
            }
            else
            {
                clickBtn.Image = null;
            }
        }
        // return Maping to Image btn
        private Image InsertImageToButton(string nameOfImg)
        {
            return new Bitmap(Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, c_ImageFolderName, nameOfImg)),
                   new Size(c_BtnWidth, c_BtnHeight));
        }
        /* Initialize Image in Form*/
        private void InitImage()
        {

            monkeySmile.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, c_ImageFolderName, c_MonkeySmileImageName));
            monkeyFail.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, c_ImageFolderName, c_MonkeyFailImageName));
            monkeySmile.Size = monkeyFail.Size = new Size(c_IconWidth, c_IconHeight);
            monkeySmile.Left = monkeyFail.Left = (this.Width - c_IconWidth) / 2;
            monkeySmile.Top = monkeyFail.Top = 20;
            monkeySmile.SizeMode = monkeyFail.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(monkeySmile);
            this.Controls.Add(monkeyFail);
        }

        private void InitInitializeComboBox()
        {
            ComboBox1 = new ComboBox();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            ComboBox1.Items.AddRange(Enum.GetNames(typeof(eGameMode)));
            this.ComboBox1.Location = new System.Drawing.Point(10, 10);
            this.ComboBox1.MaxDropDownItems = 5;
            this.ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ComboBox1.Size = new System.Drawing.Size(136, 81);
            this.ComboBox1.TabIndex = 0;
            this.ComboBox1.SelectedIndex = (int)eGameMode.NewGame;
            this.ComboBox1.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.ComboBox1.ForeColor = System.Drawing.Color.Black;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(ComboBox1_SelectedIndexChanged);
            this.Controls.Add(this.ComboBox1);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)

        {
            ComboBox cb = sender as ComboBox;

            int selectedIndex = cb.SelectedIndex;


            switch (selectedIndex)
            {
                case (int)eGameMode.NewGame:
                    m_NumOfBoombs = 5;
                    break;
                case (int)eGameMode.BeginnerMode:
                    m_NumOfBoombs = 10;
                    break;
                case (int)eGameMode.MediumMode:
                    m_NumOfBoombs = 15;
                    break;
                case (int)eGameMode.ExpertMode:
                    m_NumOfBoombs = 20;
                    break;
                case (int)eGameMode.Exit:
                    this.Close();
                    return;
            }
            StartGame(m_NumOfBoombs);
        }




    }
}
