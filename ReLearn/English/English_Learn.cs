using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Plugin.TextToSpeech;

namespace ReLearn
{
    [Activity(Label = "Learn", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Learn : Activity
    {
        TextView textView_learn_en;
        TextView textView_learn_ru;
        bool Voice_Enable = true;

        [Java.Interop.Export("Button_English_Learn_Next_Click")]
        public void Button_English_Learn_Next_Click(View v)
        {
            Following_Random_Word();              
        }

        [Java.Interop.Export("Button_English_Learn_Voice_Click")]
        public void Button_English_Learn_Voice_Click(View v)
        {
            CrossTextToSpeech.Current.Speak(textView_learn_en.Text);
        }

        [Java.Interop.Export("Button_English_Learn_Voice_Enable")]
        public void Button_English_Learn_Voice_Enable(View v)
        {
            ImageButton button = FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff);
            if (Voice_Enable)
            {
                Voice_Enable = false;
                button.SetImageDrawable(GetDrawable(Resource.Drawable.speak_off));
                Toast.MakeText(this, Additional_functions.GetResourceString("Voice_off", this.Resources), ToastLength.Short).Show();
            }
            else
            {
                Voice_Enable = true;
                button.SetImageDrawable(GetDrawable(Resource.Drawable.speak_on));
                Toast.MakeText(this, Additional_functions.GetResourceString("Voice_on", this.Resources), ToastLength.Short).Show();
            }
        }

        public void Following_Random_Word()
        {
            try
            {
                var db = DataBase.Connect(Database_Name.English_DB);
                var dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name);
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                int rand_word = rnd.Next(dataBase.Count);
                textView_learn_en.Text = dataBase[rand_word].Word;
                textView_learn_ru.Text = dataBase[rand_word].TranslationWord;

                if (Voice_Enable)
                    CrossTextToSpeech.Current.Speak(textView_learn_en.Text);          
            }
            catch
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("databaseNotConnect", this.Resources), ToastLength.Short).Show();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Learn);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            //this.ActionBar.SetBackgroundDrawable(GetDrawable(Resource.Drawable.BackgroundActionBar));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            textView_learn_en = FindViewById<TextView>(Resource.Id.textView_learn_en);
            textView_learn_ru = FindViewById<TextView>(Resource.Id.textView_learn_ru);

            Following_Random_Word();              
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
    
} 