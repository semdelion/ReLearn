namespace ReLearn.Droid
{

    static class GetString
    {
        public static string GetResourceString(string str, Android.Content.Res.Resources resource)
        {
            try
            {
                var resourceId = (int)typeof(Resource.String).GetField(str).GetValue(null);
                return resource.GetString(resourceId);
            }
            catch
            {
                return "Error: Error can't find string  - " + str;
            }
        }
        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", s.Word);
        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Word = ?", s.Word);
    }
}