using System;
using System.Collections.Generic;

namespace ChildSafeSexEducation.Mobile.Services
{
    public enum Language
    {
        English,
        Burmese
    }

    public class LanguageService
    {
        private static LanguageService? _instance;
        public static LanguageService Instance => _instance ??= new LanguageService();

        public Language CurrentLanguage { get; private set; } = Language.English;

        private readonly Dictionary<Language, Dictionary<string, string>> _translations;

        private LanguageService()
        {
            _translations = new Dictionary<Language, Dictionary<string, string>>
            {
                [Language.English] = GetEnglishTranslations(),
                [Language.Burmese] = GetBurmeseTranslations()
            };
        }

        public void SetLanguage(Language language)
        {
            CurrentLanguage = language;
        }

        public string GetText(string key)
        {
            if (_translations[CurrentLanguage].TryGetValue(key, out var text))
            {
                return text;
            }
            return key; // Return key if translation not found
        }

        private Dictionary<string, string> GetEnglishTranslations()
        {
            return new Dictionary<string, string>
            {
                // Welcome Screen
                ["welcome_title"] = "⭐ Little Star",
                ["welcome_subtitle"] = "A safe space to learn about growing up",
                ["enter_name"] = "What's your name?",
                ["select_age"] = "How old are you?",
                ["start_button"] = "Start Learning",
                ["age_8"] = "8 years old",
                ["age_9"] = "9 years old",
                ["age_10"] = "10 years old",
                ["age_11"] = "11 years old",
                ["age_12"] = "12 years old",
                ["age_13"] = "13 years old",
                ["age_14"] = "14 years old",
                ["age_15"] = "15 years old",

                // Main Screen
                ["main_title"] = "Little Star",
                ["topics_button"] = "📚 Topics",
                ["home_button"] = "🏠",
                ["chat_input_placeholder"] = "Type your message here...",
                ["send_button"] = "Send",
                ["welcome_chat_message"] = "Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!",
                ["typing_guidance_message"] = "Hi! I see you're typing, but the best way to learn is by clicking the '📚 Topics' button above! There you can explore different topics and questions. Try clicking it to see all the available learning topics! 😊",

                // Topics
                ["topics_title"] = "Learning Topics",
                ["topics_tab"] = "Topics",
                ["questions_tab"] = "Questions",
                ["blogs_tab"] = "Blogs",
                ["close_button"] = "Close",

                // Topics
                ["topic_body_parts"] = "Body Parts",
                ["topic_body_parts_desc"] = "Learn about different parts of your body",
                ["topic_personal_safety"] = "Personal Safety",
                ["topic_personal_safety_desc"] = "Understanding personal boundaries and safety",
                ["topic_growing_up"] = "Growing Up",
                ["topic_growing_up_desc"] = "Understanding changes as you grow",
                ["topic_friendships"] = "Friendships",
                ["topic_friendships_desc"] = "Learning about friendships and relationships",
                ["topic_family"] = "Family",
                ["topic_emotions"] = "Emotions",

                // Questions
                ["question_what_private_parts"] = "What are private parts?",
                ["answer_private_parts"] = "Private parts are the parts of your body that are covered by your underwear or swimsuit. These include your genitals and bottom. It's important to keep these areas clean and private.",
                ["question_why_keep_clean"] = "Why should I keep my private parts clean?",
                ["answer_keep_clean"] = "Keeping your private parts clean helps prevent infections and keeps you healthy. Always wash these areas gently with soap and water when you take a bath or shower.",
                ["question_what_private_parts_2"] = "What should I know about my private parts?",
                ["answer_private_parts_2"] = "Your private parts are special and belong only to you. No one should touch them except you (to keep clean) or a doctor (with your parent's permission). If someone tries to touch them inappropriately, tell a trusted adult immediately.",

                ["question_how_stay_safe"] = "How can I stay safe?",
                ["answer_stay_safe"] = "Always trust your feelings. If something feels wrong or uncomfortable, it probably is. Tell a trusted adult like your parents, teachers, or family members if you feel unsafe. Remember, it's never your fault if someone makes you feel uncomfortable.",
                ["question_good_bad_touch"] = "What's the difference between good touch and bad touch?",
                ["answer_good_bad_touch"] = "Good touch makes you feel safe and comfortable, like hugs from family or a doctor's gentle examination with permission. Bad touch makes you feel scared, uncomfortable, or confused. Trust your feelings and always tell a trusted adult about bad touch.",
                ["question_who_trust"] = "Who can I trust?",
                ["answer_who_trust"] = "You can trust your parents, grandparents, teachers, school counselors, and family members who make you feel safe. If someone asks you to keep secrets about touching, that's a red flag. Always tell a trusted adult about any secrets that make you uncomfortable.",

                ["question_what_changes"] = "What changes happen as I grow up?",
                ["answer_what_changes"] = "As you grow up, your body will change in many ways. You might grow taller, your voice might change, and you might develop new feelings. These changes are normal and happen to everyone at different times. It's okay to ask questions about these changes.",
                ["question_when_puberty"] = "When does puberty start?",
                ["answer_when_puberty"] = "Puberty usually starts between ages 8-13 for girls and 9-14 for boys, but everyone is different. Some people start earlier or later, and that's completely normal. Your body will let you know when it's ready for these changes.",
                ["question_why_hair_grow"] = "Why does hair grow in new places?",
                ["answer_why_hair_grow"] = "Hair grows in new places like under your arms and around your private parts as part of growing up. This is completely normal and happens to everyone. It's just your body's way of preparing for adulthood.",

                ["question_what_good_friend"] = "What makes a good friend?",
                ["answer_good_friend"] = "A good friend is someone who is kind, respectful, and makes you feel happy and safe. They listen to you, support you, and don't pressure you to do things you're not comfortable with. Good friends accept you for who you are.",
                ["question_how_handle_bullying"] = "How should I handle bullying?",
                ["answer_handle_bullying"] = "If someone is bullying you, tell a trusted adult immediately. Don't try to handle it alone. Bullies often target people who seem vulnerable, so standing up for yourself and others (with adult help) is important. Remember, bullying is never your fault.",
                ["question_what_peer_pressure"] = "What is peer pressure?",
                ["answer_peer_pressure"] = "Peer pressure is when friends or classmates try to get you to do something you don't want to do. It's okay to say no to things that make you uncomfortable. True friends will respect your decisions and won't pressure you.",

                // Dialog Messages
                ["missing_information"] = "Missing Information",
                ["missing_name_age"] = "Please enter your name and select your age.",
                ["invalid_age"] = "Invalid Age",
                ["invalid_age_message"] = "You must be between 8 and 15 years old to use this app.",
                ["invalid_email"] = "Invalid Email",
                ["invalid_email_message"] = "Please enter a valid email address.",
                ["email_recommendation"] = "Email Recommendation",
                ["email_recommendation_message"] = "We recommend using a Gmail email address for better email delivery. Would you like to change the email address?",
                ["error"] = "Error",
                ["no_user_logged_in"] = "No user logged in.",
                ["no_parent_email"] = "No parent email address provided.",
                ["success"] = "Success",
                ["daily_log_sent_success"] = "Daily log sent successfully to {0}!",
                ["error_sending_daily_log"] = "Error sending daily log: {0}",
                ["test_email_success"] = "Test Email Success",
                ["test_email_success_message"] = "✅ {0}\n\nCheck your inbox (and spam folder) if in real mode.",
                ["test_email_error"] = "Test Email Error",
                ["test_email_failed"] = "❌ Test email failed:\n\n{0}",
                ["unexpected_error"] = "❌ Unexpected error: {0}\n\nType: {1}",

                // Language Selection
                ["select_language"] = "Select Language",
                ["language_english"] = "English",
                ["language_burmese"] = "မြန်မာ",
                ["continue_button"] = "Continue"
            };
        }

        private Dictionary<string, string> GetBurmeseTranslations()
        {
            return new Dictionary<string, string>
            {
                // Welcome Screen
                ["welcome_title"] = "🌟 လုံခြုံသော သင်ယူမှု ချတ်ဘော့",
                ["welcome_subtitle"] = "ကြီးပြင်းလာခြင်းအကြောင်း လုံခြုံစွာ သင်ယူရန် နေရာ",
                ["enter_name"] = "သင့်အမည်ကား အဘယ်နည်း?",
                ["select_age"] = "သင်အသက်ကား မည်မျှရှိသနည်း?",
                ["start_button"] = "သင်ယူမှု စတင်ရန်",
                ["age_8"] = "၈ နှစ်",
                ["age_9"] = "၉ နှစ်",
                ["age_10"] = "၁၀ နှစ်",
                ["age_11"] = "၁၁ နှစ်",
                ["age_12"] = "၁၂ နှစ်",
                ["age_13"] = "၁၃ နှစ်",
                ["age_14"] = "၁၄ နှစ်",
                ["age_15"] = "၁၅ နှစ်",

                // Main Screen
                ["main_title"] = "Little Star",
                ["topics_button"] = "📚 ခေါင်းစဉ်များ",
                ["home_button"] = "🏠",
                ["chat_input_placeholder"] = "သင့်စာကို ဤနေရာတွင် ရိုက်ထည့်ပါ...",
                ["send_button"] = "ပို့ပေးရန်",
                ["welcome_chat_message"] = "မင်္ဂလာပါ! ကျွန်တော်သည် လုံခြုံသော နှင့် ကျန်းမာသော ခေါင်းစဉ်များအကြောင်း သင်ယူရန် ကူညီရန် ဤနေရာတွင် ရှိပါသည်။ သင်သည် မေးခွန်းများ မေးနိုင်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်ရှုရန် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်နိုင်ပါသည်!",
                ["typing_guidance_message"] = "မင်္ဂလာပါ! သင်ရိုက်နေသည်ကို ကျွန်တော် မြင်တွေ့ပါသည်၊ သို့သော် သင်ယူရန် အကောင်းဆုံးနည်းလမ်းမှာ အပေါ်ရှိ '📚 ခေါင်းစဉ်များ' ခလုတ်ကို နှိပ်ခြင်းဖြစ်သည်! ထိုနေရာတွင် သင်သည် ခေါင်းစဉ်များနှင့် မေးခွန်းများကို လေ့လာနိုင်သည်။ ရှိသော သင်ယူမှု ခေါင်းစဉ်များကို ကြည့်ရှုရန် ထိုခလုတ်ကို နှိပ်ကြည့်ပါ! 😊",

                // Topics
                ["topics_title"] = "သင်ယူမှု ခေါင်းစဉ်များ",
                ["topics_tab"] = "ခေါင်းစဉ်များ",
                ["questions_tab"] = "မေးခွန်းများ",
                ["blogs_tab"] = "ဘလော့များ",
                ["close_button"] = "ပိတ်ရန်",

                // Topics
                ["topic_body_parts"] = "ကိုယ်ခန္ဓာ အစိတ်အပိုင်းများ",
                ["topic_body_parts_desc"] = "သင့်ကိုယ်ခန္ဓာ၏ မတူညီသော အစိတ်အပိုင်းများအကြောင်း လေ့လာပါ",
                ["topic_personal_safety"] = "ကိုယ်ရေးကိုယ်တာ လုံခြုံမှု",
                ["topic_personal_safety_desc"] = "ကိုယ်ရေးကိုယ်တာ နယ်နိမိတ်များနှင့် လုံခြုံမှုကို နားလည်ခြင်း",
                ["topic_growing_up"] = "ကြီးပြင်းလာခြင်း",
                ["topic_growing_up_desc"] = "သင်ကြီးပြင်းလာသည်နှင့်အမျှ ပြောင်းလဲမှုများကို နားလည်ခြင်း",
                ["topic_friendships"] = "မိတ်ဆွေများ",
                ["topic_friendships_desc"] = "မိတ်ဆွေများနှင့် ဆက်ဆံရေးအကြောင်း လေ့လာခြင်း",
                ["topic_family"] = "မိသားစု",
                ["topic_emotions"] = "စိတ်ခံစားမှုများ",

                // Questions
                ["question_what_private_parts"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများကား အဘယ်နည်း?",
                ["answer_private_parts"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများသည် သင့်ကိုယ်ခန္ဓာ၏ အတွင်းခံ သို့မဟုတ် ရေကူးဝတ်စုံဖြင့် ဖုံးအုပ်ထားသော အစိတ်အပိုင်းများ ဖြစ်သည်။ ဤအစိတ်အပိုင်းများကို သန့်ရှင်းစွာ ထားရှိရန် နှင့် ကိုယ်ရေးကိုယ်တာ ထားရှိရန် အရေးကြီးပါသည်။",
                ["question_why_keep_clean"] = "ကျွန်ုပ်၏ ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများကို သန့်ရှင်းစွာ ထားရှိရန် အဘယ်ကြောင့် လိုအပ်သနည်း?",
                ["answer_keep_clean"] = "သင့်ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများကို သန့်ရှင်းစွာ ထားရှိခြင်းသည် ရောဂါပိုးများ မဝင်ရောက်စေရန် ကူညီပြီး သင့်ကို ကျန်းမာစေပါသည်။ ရေချိုးသည့်အခါ သို့မဟုတ် ရေပန်းချိုးသည့်အခါ ဤအစိတ်အပိုင်းများကို ဆပ်ပြာနှင့် ရေဖြင့် နူးညံ့စွာ ဆေးကြောပါ။",
                ["question_what_private_parts_2"] = "ကျွန်ုပ်၏ ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများအကြောင်း သိထားရမည့် အရာများကား အဘယ်နည်း?",
                ["answer_private_parts_2"] = "သင့်ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများသည် အထူးဖြစ်ပြီး သင့်ပိုင်သာ ဖြစ်သည်။ မည်သူမျှ သင့်ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများကို မထိရပါ (သန့်ရှင်းစွာ ထားရှိရန် သင့်ကိုယ်တိုင် ထိရုံမှလွဲ၍) သို့မဟုတ် ဆရာဝန် (သင့်မိဘများ၏ ခွင့်ပြုချက်ဖြင့်) မှလွဲ၍။ တစ်စုံတစ်ဦးသည် သင့်ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများကို မသင့်လျော်စွာ ထိရန် ကြိုးစားပါက ယုံကြည်ရသော လူကြီးတစ်ဦးကို ချက်ချင်း ပြောပါ။",

                ["question_how_stay_safe"] = "ကျွန်ုပ် လုံခြုံစွာ နေထိုင်နိုင်ရန် မည်သို့ လုပ်ဆောင်နိုင်သနည်း?",
                ["answer_stay_safe"] = "သင့်ခံစားချက်များကို အမြဲ ယုံကြည်ပါ။ တစ်စုံတစ်ခုက မမှန်ကန်ဟု သို့မဟုတ် မသက်မသာ ဖြစ်စေသည်ဟု ခံစားရပါက ထိုအရာသည် မမှန်ကန်နိုင်ပါ။ သင့်ကို မလုံခြုံဟု ခံစားရပါက သင့်မိဘများ၊ ဆရာများ သို့မဟုတ် မိသားစုဝင်များကဲ့သို့ ယုံကြည်ရသော လူကြီးတစ်ဦးကို ပြောပါ။ သတိရပါ၊ တစ်စုံတစ်ဦးသည် သင့်ကို မသက်မသာ ဖြစ်စေပါက ထိုအရာသည် သင့်အမှား မဟုတ်ပါ။",
                ["question_good_bad_touch"] = "ကောင်းသော ထိတွေ့မှုနှင့် မကောင်းသော ထိတွေ့မှုတို့၏ ကွာခြားချက်ကား အဘယ်နည်း?",
                ["answer_good_bad_touch"] = "ကောင်းသော ထိတွေ့မှုသည် သင့်ကို လုံခြုံပြီး သက်တောင့်သက်သာ ဖြစ်စေသည်၊ မိသားစုမှ ပွေ့ဖက်ခြင်း သို့မဟုတ် ခွင့်ပြုချက်ဖြင့် ဆရာဝန်၏ နူးညံ့သော စစ်ဆေးမှုကဲ့သို့။ မကောင်းသော ထိတွေ့မှုသည် သင့်ကို ကြောက်ရွံ့စေသည်၊ မသက်မသာ ဖြစ်စေသည် သို့မဟုတ် ရှုပ်ထွေးစေသည်။ သင့်ခံစားချက်များကို ယုံကြည်ပြီး မကောင်းသော ထိတွေ့မှုအကြောင်း ယုံကြည်ရသော လူကြီးတစ်ဦးကို အမြဲ ပြောပါ။",
                ["question_who_trust"] = "ကျွန်ုပ် မည်သူ့ကို ယုံကြည်နိုင်သနည်း?",
                ["answer_who_trust"] = "သင့်မိဘများ၊ ဘိုးဘွားများ၊ ဆရာများ၊ ကျအကြံပေးများ နှင့် သင့်ကို လုံခြုံဟု ခံစားစေသော မိသားစုဝင်များကို ယုံကြည်နိုင်ပါသည်။ တစ်စုံတစ်ဦးသည် ထိတွေ့ခြင်းအကြောင်း လျှို့ဝှက်ချက်များ ထားရန် တောင်းဆိုပါက ထိုအရာသည် အန္တရာယ်ရှိသော အချက်ပြမှု ဖြစ်သည်။ သင့်ကို မသက်မသာ ဖြစ်စေသော မည်သည့် လျှို့ဝှက်ချက်အကြောင်းမဆို ယုံကြည်ရသော လူကြီးတစ်ဦးကို အမြဲ ပြောပါ။",

                ["question_what_changes"] = "ကျွန်ုပ် ကြီးပြင်းလာသည်နှင့်အမျှ မည်သည့် ပြောင်းလဲမှုများ ဖြစ်ပေါ်သနည်း?",
                ["answer_what_changes"] = "သင်ကြီးပြင်းလာသည်နှင့်အမျှ သင့်ကိုယ်ခန္ဓာသည် နည်းလမ်းများစွာဖြင့် ပြောင်းလဲလာမည်။ သင်သည် ပိုမို ရှည်လျားလာနိုင်သည်၊ သင့်အသံ ပြောင်းလဲနိုင်သည်၊ နှင့် သင်သည် ခံစားချက်အသစ်များ ဖွံ့ဖြိုးလာနိုင်သည်။ ဤပြောင်းလဲမှုများသည် ပုံမှန်ဖြစ်ပြီး လူတိုင်းတွင် ကွဲပြားသော အချိန်များတွင် ဖြစ်ပေါ်သည်။ ဤပြောင်းလဲမှုများအကြောင်း မေးခွန်းများ မေးရန် မှန်ကန်ပါသည်။",
                ["question_when_puberty"] = "အပျိုဖော်ဝင်ခြင်း မည်သည့်အချိန်တွင် စတင်သနည်း?",
                ["answer_when_puberty"] = "အပျိုဖော်ဝင်ခြင်းသည် မိန်းကလေးများတွင် အသက် ၈-၁၃ နှစ်ကြားတွင် နှင့် ယောက်ျားလေးများတွင် အသက် ၉-၁၄ နှစ်ကြားတွင် စတင်လေ့ရှိသည်၊ သို့သော် လူတိုင်းသည် ကွဲပြားပါသည်။ အချို့လူများသည် စောစောစီးစီး သို့မဟုတ် နောက်ကျစွာ စတင်ပြီး ထိုအရာသည် လုံးဝ ပုံမှန်ဖြစ်ပါသည်။ သင့်ကိုယ်ခန္ဓာသည် ဤပြောင်းလဲမှုများအတွက် အဆင်သင့်ဖြစ်သည့်အခါ သင့်ကို သိစေမည်။",
                ["question_why_hair_grow"] = "အမွေးများ နေရာအသစ်များတွင် မည်ကြောင့် ပေါက်ရောက်သနည်း?",
                ["answer_why_hair_grow"] = "အမွေးများသည် သင့်လက်မောင်းအောက်နှင့် သင့်ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများအနီးတွင် ကြီးပြင်းလာခြင်း၏ တစ်စိတ်တစ်ပိုင်းအဖြစ် နေရာအသစ်များတွင် ပေါက်ရောက်လာသည်။ ဤအရာသည် လုံးဝ ပုံမှန်ဖြစ်ပြီး လူတိုင်းတွင် ဖြစ်ပေါ်သည်။ ထိုအရာသည် သင့်ကိုယ်ခန္ဓာ၏ လူကြီးဖြစ်လာရန် ပြင်ဆင်နည်းလမ်း ဖြစ်ပါသည်။",

                ["question_what_good_friend"] = "ကောင်းသော မိတ်ဆွေတစ်ဦးကို မည်သည့်အရာများက ဖြစ်စေသနည်း?",
                ["answer_good_friend"] = "ကောင်းသော မိတ်ဆွေတစ်ဦးသည် ကြင်နာတတ်သော၊ လေးစားတတ်သော နှင့် သင့်ကို ပျော်ရွှင်ပြီး လုံခြုံစေသော လူတစ်ဦး ဖြစ်သည်။ သူတို့သည် သင့်ကို နားထောင်ပြီး ထောက်ခံပေးပြီး သင့်ကို မသက်မသာ ဖြစ်စေသော အရာများ လုပ်ရန် ဖိအားမပေးပါ။ ကောင်းသော မိတ်ဆွေများသည် သင်ဖြစ်သည့်အတိုင်း လက်ခံပါသည်။",
                ["question_how_handle_bullying"] = "ကျွန်ုပ် အနိုင်ကျင့်ခံရခြင်းကို မည်သို့ ကိုင်တွယ်ရမည်နည်း?",
                ["answer_handle_bullying"] = "တစ်စုံတစ်ဦးသည် သင့်ကို အနိုင်ကျင့်နေပါက ယုံကြည်ရသော လူကြီးတစ်ဦးကို ချက်ချင်း ပြောပါ။ ထိုအရာကို တစ်ဦးတည်း ကိုင်တွယ်ရန် မကြိုးစားပါနှင့်။ အနိုင်ကျင့်သူများသည် မကြာခဏ အားနည်းသော လူများကို ရွေးချယ်တတ်သောကြောင့် သင့်ကိုယ်တိုင် နှင့် အခြားသူများအတွက် ရပ်တည်ခြင်း (လူကြီးများ၏ အကူအညီဖြင့်) သည် အရေးကြီးပါသည်။ သတိရပါ၊ အနိုင်ကျင့်ခံရခြင်းသည် သင့်အမှား မဟုတ်ပါ။",
                ["question_what_peer_pressure"] = "အများနှင့် ဖိအားပေးခြင်းကား အဘယ်နည်း?",
                ["answer_peer_pressure"] = "အများနှင့် ဖိအားပေးခြင်းသည် မိတ်ဆွေများ သို့မဟုတ် အတန်းဖော်များသည် သင့်ကို သင် မလုပ်လိုသော အရာများ လုပ်ရန် ကြိုးစားသည့်အခါ ဖြစ်ပေါ်သည်။ သင့်ကို မသက်မသာ ဖြစ်စေသော အရာများကို ငြင်းဆိုရန် မှန်ကန်ပါသည်။ စစ်မှန်သော မိတ်ဆွေများသည် သင့်ဆုံးဖြတ်ချက်များကို လေးစားပြီး သင့်ကို ဖိအားမပေးပါ။",

                // Dialog Messages
                ["missing_information"] = "အချက်အလက် မပြည့်စုံပါ",
                ["missing_name_age"] = "ကျေးဇူးပြု၍ သင့်အမည်နှင့် အသက်ကို ရွေးချယ်ပါ။",
                ["invalid_age"] = "မမှန်ကန်သော အသက်",
                ["invalid_age_message"] = "ဤအက်ပ်ကို အသုံးပြုရန် သင်သည် ၈ နှစ်မှ ၁၅ နှစ်ကြား ရှိရပါမည်။",
                ["invalid_email"] = "မမှန်ကန်သော အီးမေးလ်",
                ["invalid_email_message"] = "ကျေးဇူးပြု၍ မှန်ကန်သော အီးမေးလ် လိပ်စာကို ရိုက်ထည့်ပါ။",
                ["email_recommendation"] = "အီးမေးလ် အကြံပြုချက်",
                ["email_recommendation_message"] = "အီးမေးလ် ပို့ဆောင်မှု ပိုကောင်းစေရန် Gmail အီးမေးလ် လိပ်စာကို အသုံးပြုရန် အကြံပြုပါသည်။ အီးမေးလ် လိပ်စာကို ပြောင်းလဲလိုပါသလား?",
                ["error"] = "အမှား",
                ["no_user_logged_in"] = "အသုံးပြုသူ ဝင်ရောက်မထားပါ။",
                ["no_parent_email"] = "မိဘ အီးမေးလ် လိပ်စာ မပေးထားပါ။",
                ["success"] = "အောင်မြင်ပါသည်",
                ["daily_log_sent_success"] = "နေ့စဉ် လော့ဂ်ကို {0} သို့ အောင်မြင်စွာ ပို့ပြီးပါပြီ!",
                ["error_sending_daily_log"] = "နေ့စဉ် လော့ဂ် ပို့ရာတွင် အမှား: {0}",
                ["test_email_success"] = "အီးမေးလ် စမ်းသပ်မှု အောင်မြင်ပါသည်",
                ["test_email_success_message"] = "✅ {0}\n\nအမှန်တကယ် မုဒ်တွင် ရှိပါက သင့်ဝင်စာတွင် (နှင့် spam ဖိုင်တွဲ) စစ်ဆေးပါ။",
                ["test_email_error"] = "အီးမေးလ် စမ်းသပ်မှု အမှား",
                ["test_email_failed"] = "❌ အီးမေးလ် စမ်းသပ်မှု မအောင်မြင်ပါ:\n\n{0}",
                ["unexpected_error"] = "❌ မမျှော်လင့်သော အမှား: {0}\n\nအမျိုးအစား: {1}",

                // Language Selection
                ["select_language"] = "ဘာသာစကား ရွေးချယ်ပါ",
                ["language_english"] = "English",
                ["language_burmese"] = "မြန်မာ",
                ["continue_button"] = "ဆက်လက်လုပ်ဆောင်ရန်"
            };
        }
    }
}
