using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Calligraphy;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_SelectDictionary : AppCompatActivity
    {
        SelectDictionary Dictionaries { get; set; }

        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Dictionaries.Selected(ImgV.Tag.ToString(), DataBase.TableNameImage.ToString());
            Enum.TryParse(ImgV.Tag.ToString(), out TableNamesImage name);
            DataBase.TableNameImage = name;
            DataBase.UpdateImagesToRepeat();
        }

        public void CreateViewForDictionary(string name, int ImageId)
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);
            List<Database_for_stats> Database_NL_and_D = database.Query<Database_for_stats>("SELECT NumberLearn, DateRecurrence FROM " + name.ToString());

            Dictionaries.DictionariesBitmap.Add(Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), Database_NL_and_D));

            ImageView DictionaryImage = new ImageView(this)
            {
                LayoutParameters = Dictionaries.parmsImage,
                Tag = name
            };
            DictionaryImage.SetImageBitmap(Dictionaries.DictionariesBitmap[Dictionaries.DictionariesBitmap.Count - 1]);
            DictionaryImage.Click += SelectDictionaryClick;
            FindViewById<LinearLayout>(Resource.Id.FlagsSelectDictionary).AddView(DictionaryImage);

            TextView DictionaryName = new TextView(this)
            {
                Text = Additional_functions.GetResourceString(name, this.Resources),
                Gravity = Dictionaries.parmsImage.Gravity
            };
            DictionaryName.SetTextColor(Color.Rgb(215, 248, 254));
            FindViewById<LinearLayout>(Resource.Id.FlagsSelectDictionary).AddView(DictionaryName);
            Dictionaries.DictionariesView.Add(DictionaryImage);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_SelectDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsSelectDictionary);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Dictionaries = new SelectDictionary((int)(Resources.DisplayMetrics.WidthPixels / 3f));
            CreateViewForDictionary(TableNamesImage.Flags.ToString(), Resource.Drawable.FlagDictionary);
            Dictionaries.Selected(DataBase.TableNameImage.ToString(), DataBase.TableNameImage.ToString());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }
        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}