using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChildSafeSexEducation.Desktop.Services
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
                ["enter_name"] = "Enter your name:",
                ["select_age"] = "Select your age:",
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

                // Topics Popup
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
                ["question_why_keep_clean"] = "Why is it important to keep our bodies clean?",
                ["question_what_private_parts_2"] = "What are private parts?",
                ["question_how_stay_safe"] = "How can I stay safe?",
                ["question_good_bad_touch"] = "What are good touch and bad touch?",
                ["question_who_trust"] = "Who can you trust to talk about personal problems?",
                ["question_what_puberty"] = "What is puberty?",
                ["question_confused_changes"] = "Is it normal to feel confused about changes in my body?",
                ["question_when_talk_changes"] = "When should I talk to someone about body changes?",
                ["question_how_make_friends"] = "How do I make friends?",
                ["question_someone_mean"] = "How do you know if someone is being mean to you?",
                ["question_bullying_help"] = "What should you do if someone is bullying you?",
                ["question_what_family_love"] = "What is family love?",
                ["question_how_handle_emotions"] = "How do I handle my emotions?",

                // Answers
                ["answer_private_parts"] = "Private parts are the parts of your body that are covered by your underwear or swimsuit. These are special parts that only you, your parents, and doctors should see.",
                ["answer_stay_safe"] = "You can stay safe by: 1) Always telling a trusted adult if someone makes you uncomfortable, 2) Remembering that your body belongs to you, 3) Knowing it's okay to say 'no' to things that feel wrong.",
                ["answer_puberty"] = "Puberty is when your body starts changing as you grow older. You might grow taller, your voice might change, and you might have new feelings. This is completely normal!",
                ["answer_make_friends"] = "You can make friends by being kind, sharing, listening to others, and being yourself. Good friends respect you and make you feel happy and safe.",
                ["answer_family_love"] = "Family love is the special bond between family members. It means caring for each other, supporting each other, and being there when someone needs help.",
                ["answer_handle_emotions"] = "It's normal to have different emotions. You can handle them by talking to someone you trust, taking deep breaths, doing activities you enjoy, and remembering that feelings change.",
                ["answer_keep_clean"] = "Keeping our bodies clean helps prevent germs and keeps us healthy. It's important to wash regularly and take care of our skin.",
                ["answer_private_parts_2"] = "Private parts are the parts of our body that are covered by underwear or swimsuits. These are special parts that should be kept private and only touched by yourself or a doctor when needed.",
                ["answer_good_bad_touch"] = "Good touch makes you feel safe and comfortable, like hugs from family or a doctor's gentle examination. Bad touch makes you feel scared, uncomfortable, or confused.",
                ["answer_who_trust"] = "You can trust parents, teachers, school counselors, doctors, and other trusted adults who care about your safety and well-being.",
                ["answer_confused_changes"] = "Yes, it's completely normal to feel confused or worried about changes in your body. Everyone goes through these changes at their own pace.",
                ["answer_when_talk_changes"] = "You should talk to a trusted adult whenever you have questions or concerns about changes in your body. It's always okay to ask questions.",
                ["answer_someone_mean"] = "If someone is calling you names, excluding you, hurting you physically or emotionally, or making you feel bad about yourself, they are being mean.",
                ["answer_bullying_help"] = "Tell a trusted adult immediately. Bullying is never okay, and adults can help stop it. You are not alone, and it's not your fault.",

                // Related Blogs
                ["related_articles"] = "📚 Related Articles",
                ["blog_body_awareness"] = "Understanding Your Body",
                ["blog_body_awareness_desc"] = "Learn about body parts and keeping clean",
                ["blog_safety_rules"] = "Staying Safe",
                ["blog_safety_rules_desc"] = "Important safety tips for children",
                ["blog_growing_changes"] = "Growing Up Changes",
                ["blog_growing_changes_desc"] = "What to expect as you grow older",
                ["blog_healthy_friendships"] = "Making Good Friends",
                ["blog_healthy_friendships_desc"] = "How to build healthy friendships",
                ["blog_talking_adults"] = "Talking to Adults",
                ["blog_talking_adults_desc"] = "When and how to ask for help",
                ["blog_body_boundaries"] = "Body Boundaries",
                ["blog_body_boundaries_desc"] = "Understanding personal space and privacy",
                ["blog_family_bonds"] = "Family Bonds",
                ["blog_emotional_health"] = "Emotional Health",

                // Blog Content Sections
                ["blog_body_parts_what"] = "What are Body Parts?",
                ["blog_body_parts_what_content"] = "Your body has many different parts, each with its own important job. Some parts are on the outside, like your arms, legs, and face. Others are on the inside, like your heart, lungs, and stomach.",
                ["blog_body_parts_clean"] = "Keeping Clean",
                ["blog_body_parts_clean_content"] = "It's important to keep all parts of your body clean. This means taking regular baths or showers, washing your hands, and brushing your teeth. Clean bodies stay healthy!",
                ["blog_body_parts_privacy"] = "Privacy",
                ["blog_body_parts_privacy_content"] = "Some parts of your body are private. These are parts that are usually covered by clothes. It's okay to talk about these parts with trusted adults like doctors or parents when you need help.",

                ["blog_safety_what"] = "What is Personal Safety?",
                ["blog_safety_what_content"] = "Personal safety means knowing how to keep yourself safe and comfortable. It's about understanding your boundaries and knowing when something doesn't feel right.",
                ["blog_safety_touches"] = "Safe vs Unsafe Touches",
                ["blog_safety_touches_content"] = "Safe touches make you feel good and comfortable, like hugs from family or high-fives from friends. Unsafe touches make you feel uncomfortable, scared, or confused.",
                ["blog_safety_saying_no"] = "Saying No",
                ["blog_safety_saying_no_content"] = "You always have the right to say 'no' if someone tries to touch you in a way that makes you uncomfortable. It's important to tell a trusted adult if this happens.",

                ["blog_growing_gradually"] = "Changes Happen Gradually",
                ["blog_growing_gradually_content"] = "As you grow up, your body will change slowly over time. These changes are normal and happen to everyone at different times.",
                ["blog_growing_physical"] = "Physical Changes",
                ["blog_growing_physical_content"] = "You might notice changes like getting taller, your voice changing, or new hair growing in different places. These are all normal parts of growing up.",
                ["blog_growing_emotional"] = "Emotional Changes",
                ["blog_growing_emotional_content"] = "You might also feel different emotions or have new thoughts as you get older. This is completely normal and part of becoming an adult.",

                ["blog_relationships_friends"] = "What Makes a Good Friend?",
                ["blog_relationships_friends_content"] = "Good friends are kind, respectful, and make you feel happy and safe. They listen to you, share with you, and help you when you need it.",
                ["blog_relationships_respect"] = "Respecting Others",
                ["blog_relationships_respect_content"] = "In healthy relationships, people respect each other's feelings, boundaries, and personal space. Everyone should feel comfortable and valued.",
                ["blog_relationships_communication"] = "Communication",
                ["blog_relationships_communication_content"] = "Good relationships are built on honest communication. It's important to talk about your feelings and listen to others when they share theirs.",

                ["blog_default_learning"] = "Learning Together",
                ["blog_default_learning_content"] = "This topic is about learning important things in a safe and comfortable way. Always remember that it's okay to ask questions!",
                ["blog_default_adults"] = "Trusted Adults",
                ["blog_default_adults_content"] = "There are many trusted adults in your life who can help you learn and answer your questions. These include parents, teachers, doctors, and family members.",
                ["blog_default_feelings"] = "Your Feelings Matter",
                ["blog_default_feelings_content"] = "How you feel about things is important. If something makes you uncomfortable or confused, it's always okay to talk to a trusted adult about it.",

                // Blog Categories
                ["category_body_parts"] = "Body Parts",
                ["category_personal_safety"] = "Personal Safety",
                ["category_growing_up"] = "Growing Up",
                ["category_healthy_relationships"] = "Healthy Relationships",
                ["category_friendships"] = "Friendships",
                ["category_family"] = "Family",
                ["category_emotions"] = "Emotions",

                // Parent Information
                ["parent_information"] = "Parent/Guardian Information (Optional)",
                ["parent_name"] = "Parent/Guardian Name",
                ["parent_email"] = "Parent/Guardian Email",
                ["email_notifications"] = "Send daily learning reports to parent email",
                ["send_daily_log"] = "📧 Send Daily Log",
                ["sending_daily_log"] = "📧 Sending daily log to parent email...",
                ["daily_log_sent"] = "✅ Daily log sent successfully to parent email!",
                ["daily_log_error"] = "❌ Error sending daily log: {0}",

                // Messages
                ["typing_guidance"] = "Hi! I see you're typing, but the best way to learn is by clicking the '📚 Topics' button above! There you can explore different topics and questions. Try clicking it to see all the available learning topics! 😊",
                ["back_to_start_confirm"] = "Are you sure you want to go back to the start page?",
                ["back_to_start_title"] = "Back to Start",

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
                ["enter_name"] = "သင့်အမည်ကို ရိုက်ထည့်ပါ:",
                ["select_age"] = "သင့်အသက်ကို ရွေးချယ်ပါ:",
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
                ["main_title"] = "လုံခြုံသော သင်ယူမှု ချတ်ဘော့",
                ["topics_button"] = "📚 ခေါင်းစဉ်များ",
                ["home_button"] = "🏠",
                ["chat_input_placeholder"] = "သင့်စာကို ဤနေရာတွင် ရိုက်ထည့်ပါ...",
                ["send_button"] = "ပို့ရန်",
                ["welcome_chat_message"] = "မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!",
                ["typing_guidance_message"] = "မင်္ဂလာပါ! သင်ရိုက်နေတာကို မြင်ရပါတယ်၊ သို့သော် အကောင်းဆုံး သင်ယူနည်းမှာ အပေါ်ရှိ '📚 ခေါင်းစဉ်များ' ခလုတ်ကို နှိပ်ခြင်းပါ! ထိုနေရာတွင် မတူညီသော ခေါင်းစဉ်များနှင့် မေးခွန်းများကို လေ့လာနိုင်ပါတယ်။ ရနိုင်သော သင်ယူမှု ခေါင်းစဉ်များကို ကြည့်ရန် ထိုခလုတ်ကို နှိပ်ကြည့်ပါ! 😊",

                // Topics Popup
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
                ["question_what_private_parts"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများဆိုတာ ဘာလဲ?",
                ["question_why_keep_clean"] = "ကျွန်တော်တို့ ကိုယ်ခန္ဓာကို သန့်ရှင်းစွာ ထားရခြင်း အရေးကြီးတာ ဘာကြောင့်လဲ?",
                ["question_what_private_parts_2"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများဆိုတာ ဘာလဲ?",
                ["question_how_stay_safe"] = "ကျွန်တော် ဘယ်လို လုံခြုံစွာ နေနိုင်မလဲ?",
                ["question_good_bad_touch"] = "ကောင်းသော ထိတွေ့မှုနှင့် မကောင်းသော ထိတွေ့မှုဆိုတာ ဘာလဲ?",
                ["question_who_trust"] = "ကိုယ်ရေးကိုယ်တာ ပြဿနာများအကြောင်း ပြောရန် ဘယ်သူကို ယုံကြည်နိုင်မလဲ?",
                ["question_what_puberty"] = "အရွယ်ရောက်ခြင်းဆိုတာ ဘာလဲ?",
                ["question_confused_changes"] = "ကျွန်တော့် ကိုယ်ခန္ဓာ ပြောင်းလဲမှုများအကြောင်း ရှုပ်ထွေးခံစားရခြင်းသည် ပုံမှန်လား?",
                ["question_when_talk_changes"] = "ကိုယ်ခန္ဓာ ပြောင်းလဲမှုများအကြောင်း တစ်စုံတစ်ယောက်နှင့် ဘယ်အချိန်တွင် ပြောသင့်မလဲ?",
                ["question_how_make_friends"] = "ကျွန်တော် မိတ်ဆွေတွေ ဘယ်လို လုပ်နိုင်မလဲ?",
                ["question_someone_mean"] = "တစ်စုံတစ်ယောက်က သင့်ကို မကောင်းသော လုပ်ရပ်များ လုပ်နေတာကို ဘယ်လို သိနိုင်မလဲ?",
                ["question_bullying_help"] = "တစ်စုံတစ်ယောက်က သင့်ကို အနိုင်ကျင့်နေပါက ဘာလုပ်သင့်မလဲ?",
                ["question_what_family_love"] = "မိသားစု ချစ်ခြင်းမေတ္တာဆိုတာ ဘာလဲ?",
                ["question_how_handle_emotions"] = "ကျွန်တော့် စိတ်ခံစားမှုတွေကို ဘယ်လို ကိုင်တွယ်နိုင်မလဲ?",

                // Answers
                ["answer_private_parts"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများဆိုတာ သင့်ကိုယ်ခန္ဓာ၏ အစိတ်အပိုင်းများဖြစ်ပြီး အတွင်းခံ သို့မဟုတ် ရေကူးဝတ်စုံဖြင့် ဖုံးအုပ်ထားသော အစိတ်အပိုင်းများဖြစ်သည်။ ဤအစိတ်အပိုင်းများသည် သင့်အတွက်၊ သင့်မိဘများနှင့် ဆရာဝန်များသာ ကြည့်ရှုသင့်သော အထူးအစိတ်အပိုင်းများဖြစ်သည်။",
                ["answer_stay_safe"] = "သင်သည် အောက်ပါနည်းလမ်းများဖြင့် လုံခြုံစွာ နေနိုင်သည်: ၁) တစ်စုံတစ်ယောက်က သင့်ကို မသက်မသာ ဖြစ်စေပါက ယုံကြည်ရသော လူကြီးတစ်ဦးကို အမြဲပြောပါ၊ ၂) သင့်ကိုယ်ခန္ဓာသည် သင့်ပိုင်ဆိုင်မှုဖြစ်ကြောင်း မှတ်ထားပါ၊ ၃) မှားယွင်းသောအရာများကို 'မလုပ်ပါ' ဟု ပြောရဲကြောင်း သိထားပါ။",
                ["answer_puberty"] = "အရွယ်ရောက်ခြင်းဆိုတာ သင်ကြီးပြင်းလာသည့်အခါ သင့်ကိုယ်ခန္ဓာ စတင်ပြောင်းလဲခြင်းဖြစ်သည်။ သင်သည် ပိုမိုမြင့်မားလာနိုင်သည်၊ သင့်အသံ ပြောင်းလဲနိုင်သည်၊ နှင့် သင်သည် စိတ်ခံစားမှုအသစ်များ ရှိနိုင်သည်။ ဤအရာသည် လုံးဝ ပုံမှန်ဖြစ်သည်!",
                ["answer_make_friends"] = "သင်သည် ကြင်နာခြင်း၊ မျှဝေခြင်း၊ အခြားသူများကို နားထောင်ခြင်း၊ နှင့် သင့်ကိုယ်သင် ဖြစ်ခြင်းဖြင့် မိတ်ဆွေများ လုပ်နိုင်သည်။ ကောင်းသော မိတ်ဆွေများသည် သင့်ကို လေးစားပြီး သင့်ကို ပျော်ရွှင်ပြီး လုံခြုံသော ခံစားမှု ပေးသည်။",
                ["answer_family_love"] = "မိသားစု ချစ်ခြင်းမေတ္တာဆိုတာ မိသားစုဝင်များအကြား ရှိသော အထူးနှောင်ကြိုးဖြစ်သည်။ ဤအရာသည် တစ်ဦးကိုတစ်ဦး ဂရုစိုက်ခြင်း၊ တစ်ဦးကိုတစ်ဦး ပံ့ပိုးခြင်း၊ နှင့် တစ်စုံတစ်ယောက် အကူအညီလိုအပ်သည့်အခါ ရှိနေခြင်းကို ဆိုလိုသည်။",
                ["answer_handle_emotions"] = "စိတ်ခံစားမှုအမျိုးမျိုး ရှိခြင်းသည် ပုံမှန်ဖြစ်သည်။ သင်သည် ယုံကြည်ရသော လူတစ်ဦးနှင့် စကားပြောခြင်း၊ နက်ရှိုင်းစွာ အသက်ရှူခြင်း၊ သင်နှစ်သက်သော လုပ်ဆောင်ချက်များ လုပ်ခြင်း၊ နှင့် စိတ်ခံစားမှုများသည် ပြောင်းလဲကြောင်း မှတ်ထားခြင်းဖြင့် ဤအရာများကို ကိုင်တွယ်နိုင်သည်။",
                ["answer_keep_clean"] = "ကျွန်တော်တို့ ကိုယ်ခန္ဓာကို သန့်ရှင်းစွာ ထားခြင်းသည် ရောဂါပိုးများကို ကာကွယ်ပြီး ကျွန်တော်တို့ကို ကျန်းမာစေသည်။ ပုံမှန် ရေချိုးခြင်းနှင့် ကျွန်တော်တို့၏ အရေပြားကို ဂရုစိုက်ခြင်းသည် အရေးကြီးသည်။",
                ["answer_private_parts_2"] = "ကိုယ်ရေးကိုယ်တာ အစိတ်အပိုင်းများဆိုတာ ကျွန်တော်တို့ ကိုယ်ခန္ဓာ၏ အစိတ်အပိုင်းများဖြစ်ပြီး အတွင်းခံ သို့မဟုတ် ရေကူးဝတ်စုံဖြင့် ဖုံးအုပ်ထားသော အစိတ်အပိုင်းများဖြစ်သည်။ ဤအစိတ်အပိုင်းများသည် ကိုယ်ရေးကိုယ်တာ ထားရမည့် အထူးအစိတ်အပိုင်းများဖြစ်ပြီး သင့်ကိုယ်တိုင် သို့မဟုတ် လိုအပ်သည့်အခါ ဆရာဝန်များသာ ထိတွေ့သင့်သည်။",
                ["answer_good_bad_touch"] = "ကောင်းသော ထိတွေ့မှုသည် သင့်ကို လုံခြုံပြီး သက်တောင့်သက်သာ ခံစားစေသည်၊ မိသားစုမှ ပွေ့ဖက်ခြင်း သို့မဟုတ် ဆရာဝန်၏ နူးညံ့သော စစ်ဆေးခြင်းကဲ့သို့ဖြစ်သည်။ မကောင်းသော ထိတွေ့မှုသည် သင့်ကို ကြောက်ရွံ့၊ မသက်မသာ၊ သို့မဟုတ် ရှုပ်ထွေးစေသည်။",
                ["answer_who_trust"] = "သင်သည် မိဘများ၊ ဆရာများ၊ ကျောင်း အကြံပေးများ၊ ဆရာဝန်များ၊ နှင့် သင့်လုံခြုံမှုနှင့် ကောင်းကျိုးကို ဂရုစိုက်သော အခြား ယုံကြည်ရသော လူကြီးများကို ယုံကြည်နိုင်သည်။",
                ["answer_confused_changes"] = "ဟုတ်ကဲ့၊ သင့်ကိုယ်ခန္ဓာ ပြောင်းလဲမှုများအကြောင်း ရှုပ်ထွေးခံစားရခြင်း သို့မဟုတ် စိုးရိမ်ခြင်းသည် လုံးဝ ပုံမှန်ဖြစ်သည်။ လူတိုင်းသည် ဤပြောင်းလဲမှုများကို သူတို့၏ ကိုယ်ပိုင် အရှိန်ဖြင့် ဖြတ်သန်းကြသည်။",
                ["answer_when_talk_changes"] = "သင်သည် သင့်ကိုယ်ခန္ဓာ ပြောင်းလဲမှုများအကြောင်း မေးခွန်းများ သို့မဟုတ် စိုးရိမ်မှုများ ရှိသည့်အခါတိုင်း ယုံကြည်ရသော လူကြီးတစ်ဦးနှင့် ပြောသင့်သည်။ မေးခွန်းများ မေးခြင်းသည် အမြဲတမ်း ကောင်းသည်။",
                ["answer_someone_mean"] = "တစ်စုံတစ်ယောက်က သင့်ကို အမည်များ ခေါ်ခြင်း၊ သင့်ကို ဖယ်ရှားခြင်း၊ သင့်ကို ရုပ်ပိုင်းဆိုင်ရာ သို့မဟုတ် စိတ်ပိုင်းဆိုင်ရာ ထိခိုက်စေခြင်း၊ သို့မဟုတ် သင့်ကို မကောင်းသော ခံစားမှု ပေးခြင်းပါက၊ သူတို့သည် မကောင်းသော လုပ်ရပ်များ လုပ်နေခြင်းဖြစ်သည်။",
                ["answer_bullying_help"] = "ယုံကြည်ရသော လူကြီးတစ်ဦးကို ချက်ချင်း ပြောပါ။ အနိုင်ကျင့်ခြင်းသည် ဘယ်တော့မှ ကောင်းသော အရာမဟုတ်ပါ၊ နှင့် လူကြီးများသည် ဤအရာကို ရပ်တန့်ရန် ကူညီနိုင်သည်။ သင်သည် တစ်ယောက်တည်း မဟုတ်ပါ၊ နှင့် ဤအရာသည် သင့်အမှားမဟုတ်ပါ။",

                // Related Blogs
                ["related_articles"] = "📚 ဆက်စပ်သော ဆောင်းပါးများ",
                ["blog_body_awareness"] = "သင့်ကိုယ်ခန္ဓာကို နားလည်ခြင်း",
                ["blog_body_awareness_desc"] = "ကိုယ်ခန္ဓာ အစိတ်အပိုင်းများနှင့် သန့်ရှင်းစွာ ထားခြင်းအကြောင်း လေ့လာပါ",
                ["blog_safety_rules"] = "လုံခြုံစွာ နေထိုင်ခြင်း",
                ["blog_safety_rules_desc"] = "ကလေးများအတွက် အရေးကြီးသော လုံခြုံမှု အကြံပြုချက်များ",
                ["blog_growing_changes"] = "ကြီးပြင်းလာခြင်း ပြောင်းလဲမှုများ",
                ["blog_growing_changes_desc"] = "သင်ကြီးပြင်းလာသည်နှင့်အမျှ မျှော်လင့်ရမည့် အရာများ",
                ["blog_healthy_friendships"] = "ကောင်းသော မိတ်ဆွေများ လုပ်ခြင်း",
                ["blog_healthy_friendships_desc"] = "ကျန်းမာသော မိတ်ဆွေများ ဖွဲ့ခြင်း နည်းလမ်းများ",
                ["blog_talking_adults"] = "လူကြီးများနှင့် စကားပြောခြင်း",
                ["blog_talking_adults_desc"] = "အကူအညီ တောင်းခံရန် ဘယ်အချိန်နှင့် ဘယ်လို လုပ်ရမည်နည်း",
                ["blog_body_boundaries"] = "ကိုယ်ခန္ဓာ နယ်နိမိတ်များ",
                ["blog_body_boundaries_desc"] = "ကိုယ်ရေးကိုယ်တာ နေရာနှင့် လျှို့ဝှက်မှုကို နားလည်ခြင်း",
                ["blog_family_bonds"] = "မိသားစု နှောင်ကြိုးများ",
                ["blog_emotional_health"] = "စိတ်ပိုင်းဆိုင်ရာ ကျန်းမာရေး",

                // Blog Content Sections
                ["blog_body_parts_what"] = "ကိုယ်ခန္ဓာ အစိတ်အပိုင်းများဆိုတာ ဘာလဲ?",
                ["blog_body_parts_what_content"] = "သင့်ကိုယ်ခန္ဓာတွင် မတူညီသော အစိတ်အပိုင်းများစွာ ရှိပြီး၊ တစ်ခုစီတွင် သူ့၏ ကိုယ်ပိုင် အရေးကြီးသော အလုပ်များ ရှိသည်။ အချို့အစိတ်အပိုင်းများသည် အပြင်ဘက်တွင် ရှိသည်၊ သင့်လက်များ၊ ခြေထောက်များ၊ နှင့် မျက်နှာကဲ့သို့ဖြစ်သည်။ အခြားအစိတ်အပိုင်းများသည် အတွင်းဘက်တွင် ရှိသည်၊ သင့်နှလုံး၊ အဆုတ်များ၊ နှင့် အစာအိမ်ကဲ့သို့ဖြစ်သည်။",
                ["blog_body_parts_clean"] = "သန့်ရှင်းစွာ ထားခြင်း",
                ["blog_body_parts_clean_content"] = "သင့်ကိုယ်ခန္ဓာ၏ အစိတ်အပိုင်းအားလုံးကို သန့်ရှင်းစွာ ထားခြင်းသည် အရေးကြီးသည်။ ဤအရာသည် ပုံမှန် ရေချိုးခြင်း သို့မဟုတ် ရေပန်းချိုးခြင်း၊ လက်များ ဆေးခြင်း၊ နှင့် သွားများ ပွတ်တိုက်ခြင်းကို ဆိုလိုသည်။ သန့်ရှင်းသော ကိုယ်ခန္ဓာများသည် ကျန်းမာစွာ နေကြသည်!",
                ["blog_body_parts_privacy"] = "ကိုယ်ရေးကိုယ်တာ",
                ["blog_body_parts_privacy_content"] = "သင့်ကိုယ်ခန္ဓာ၏ အချို့အစိတ်အပိုင်းများသည် ကိုယ်ရေးကိုယ်တာ ဖြစ်သည်။ ဤအစိတ်အပိုင်းများသည် ပုံမှန်အားဖြင့် အဝတ်အစားများဖြင့် ဖုံးအုပ်ထားသော အစိတ်အပိုင်းများဖြစ်သည်။ သင်အကူအညီလိုအပ်သည့်အခါ ဆရာဝန်များ သို့မဟုတ် မိဘများကဲ့သို့ ယုံကြည်ရသော လူကြီးများနှင့် ဤအစိတ်အပိုင်းများအကြောင်း ပြောခြင်းသည် ကောင်းသည်။",

                ["blog_safety_what"] = "ကိုယ်ရေးကိုယ်တာ လုံခြုံမှုဆိုတာ ဘာလဲ?",
                ["blog_safety_what_content"] = "ကိုယ်ရေးကိုယ်တာ လုံခြုံမှုဆိုတာ သင့်ကိုယ်သင် လုံခြုံပြီး သက်တောင့်သက်သာ ထားနည်းကို သိခြင်းကို ဆိုလိုသည်။ ဤအရာသည် သင့်နယ်နိမိတ်များကို နားလည်ခြင်းနှင့် တစ်စုံတစ်ရာ မှန်ကန်မှု မရှိသည့်အခါ သိခြင်းအကြောင်း ဖြစ်သည်။",
                ["blog_safety_touches"] = "လုံခြုံသော နှင့် မလုံခြုံသော ထိတွေ့မှုများ",
                ["blog_safety_touches_content"] = "လုံခြုံသော ထိတွေ့မှုများသည် သင့်ကို ကောင်းမွန်ပြီး သက်တောင့်သက်သာ ခံစားစေသည်၊ မိသားစုမှ ပွေ့ဖက်ခြင်း သို့မဟုတ် မိတ်ဆွေများမှ လက်ခုပ်တီးခြင်းကဲ့သို့ဖြစ်သည်။ မလုံခြုံသော ထိတွေ့မှုများသည် သင့်ကို မသက်မသာ၊ ကြောက်ရွံ့၊ သို့မဟုတ် ရှုပ်ထွေးစေသည်။",
                ["blog_safety_saying_no"] = "မလုပ်ပါ ဟု ပြောခြင်း",
                ["blog_safety_saying_no_content"] = "တစ်စုံတစ်ယောက်က သင့်ကို မသက်မသာ ဖြစ်စေသော နည်းလမ်းဖြင့် ထိတွေ့ရန် ကြိုးစားပါက 'မလုပ်ပါ' ဟု ပြောရန် အခွင့်အရေး သင့်တွင် အမြဲရှိသည်။ ဤသို့ဖြစ်ပါက ယုံကြည်ရသော လူကြီးတစ်ဦးကို ပြောခြင်းသည် အရေးကြီးသည်။",

                ["blog_growing_gradually"] = "ပြောင်းလဲမှုများ တဖြည်းဖြည်း ဖြစ်ပေါ်ခြင်း",
                ["blog_growing_gradually_content"] = "သင်ကြီးပြင်းလာသည့်အခါ၊ သင့်ကိုယ်ခန္ဓာသည် အချိန်ကြာလာသည်နှင့်အမျှ ဖြည်းဖြည်းချင်း ပြောင်းလဲလာမည်။ ဤပြောင်းလဲမှုများသည် ပုံမှန်ဖြစ်ပြီး လူတိုင်းတွင် မတူညီသော အချိန်များတွင် ဖြစ်ပေါ်သည်။",
                ["blog_growing_physical"] = "ရုပ်ပိုင်းဆိုင်ရာ ပြောင်းလဲမှုများ",
                ["blog_growing_physical_content"] = "သင်သည် ပိုမိုမြင့်မားလာခြင်း၊ သင့်အသံ ပြောင်းလဲခြင်း၊ သို့မဟုတ် မတူညီသော နေရာများတွင် အမွေးအမျှင်အသစ်များ ပေါက်ခြင်းကဲ့သို့ ပြောင်းလဲမှုများကို သတိပြုမိနိုင်သည်။ ဤအရာများအားလုံးသည် ကြီးပြင်းလာခြင်း၏ ပုံမှန် အစိတ်အပိုင်းများဖြစ်သည်။",
                ["blog_growing_emotional"] = "စိတ်ပိုင်းဆိုင်ရာ ပြောင်းလဲမှုများ",
                ["blog_growing_emotional_content"] = "သင်သည် ကြီးပြင်းလာသည့်အခါ မတူညီသော စိတ်ခံစားမှုများ သို့မဟုတ် စိတ်ကူးအသစ်များ ရှိနိုင်သည်။ ဤအရာသည် လုံးဝ ပုံမှန်ဖြစ်ပြီး လူကြီးဖြစ်လာခြင်း၏ အစိတ်အပိုင်းဖြစ်သည်။",

                ["blog_relationships_friends"] = "ကောင်းသော မိတ်ဆွေတစ်ဦးကို ဘာက ဖြစ်စေသနည်း?",
                ["blog_relationships_friends_content"] = "ကောင်းသော မိတ်ဆွေများသည် ကြင်နာ၊ လေးစားပြီး သင့်ကို ပျော်ရွှင်ပြီး လုံခြုံသော ခံစားမှု ပေးသည်။ သူတို့သည် သင့်ကို နားထောင်သည်၊ သင့်နှင့် မျှဝေသည်၊ နှင့် သင်အကူအညီလိုအပ်သည့်အခါ ကူညီသည်။",
                ["blog_relationships_respect"] = "အခြားသူများကို လေးစားခြင်း",
                ["blog_relationships_respect_content"] = "ကျန်းမာသော ဆက်ဆံရေးများတွင်၊ လူများသည် တစ်ဦးကိုတစ်ဦး၏ စိတ်ခံစားမှုများ၊ နယ်နိမိတ်များ၊ နှင့် ကိုယ်ရေးကိုယ်တာ နေရာများကို လေးစားကြသည်။ လူတိုင်းသည် သက်တောင့်သက်သာ နှင့် တန်ဖိုးရှိသော ခံစားမှု ရှိသင့်သည်။",
                ["blog_relationships_communication"] = "ဆက်သွယ်ရေး",
                ["blog_relationships_communication_content"] = "ကောင်းသော ဆက်ဆံရေးများသည် ရိုးသားသော ဆက်သွယ်ရေးအပေါ် တည်ဆောက်ထားသည်။ သင့်စိတ်ခံစားမှုများအကြောင်း ပြောခြင်းနှင့် အခြားသူများ သူတို့၏ စိတ်ခံစားမှုများကို မျှဝေသည့်အခါ နားထောင်ခြင်းသည် အရေးကြီးသည်။",

                ["blog_default_learning"] = "အတူတကွ သင်ယူခြင်း",
                ["blog_default_learning_content"] = "ဤခေါင်းစဉ်သည် လုံခြုံပြီး သက်တောင့်သက်သာ နည်းလမ်းဖြင့် အရေးကြီးသော အရာများကို သင်ယူခြင်းအကြောင်း ဖြစ်သည်။ မေးခွန်းများ မေးခြင်းသည် ကောင်းသည်ကို အမြဲ မှတ်ထားပါ!",
                ["blog_default_adults"] = "ယုံကြည်ရသော လူကြီးများ",
                ["blog_default_adults_content"] = "သင့်ဘဝတွင် သင့်ကို သင်ယူရန် နှင့် သင့်မေးခွန်းများကို ဖြေကြားရန် ကူညီနိုင်သော ယုံကြည်ရသော လူကြီးများ များစွာ ရှိသည်။ ဤအရာများတွင် မိဘများ၊ ဆရာများ၊ ဆရာဝန်များ၊ နှင့် မိသားစုဝင်များ ပါဝင်သည်။",
                ["blog_default_feelings"] = "သင့်စိတ်ခံစားမှုများ အရေးကြီးသည်",
                ["blog_default_feelings_content"] = "အရာများအကြောင်း သင်ခံစားရသော နည်းလမ်းသည် အရေးကြီးသည်။ တစ်စုံတစ်ရာက သင့်ကို မသက်မသာ သို့မဟုတ် ရှုပ်ထွေးစေပါက၊ ယုံကြည်ရသော လူကြီးတစ်ဦးနှင့် ဤအရာအကြောင်း ပြောခြင်းသည် အမြဲတမ်း ကောင်းသည်။",

                // Blog Categories
                ["category_body_parts"] = "ကိုယ်ခန္ဓာ အစိတ်အပိုင်းများ",
                ["category_personal_safety"] = "ကိုယ်ရေးကိုယ်တာ လုံခြုံမှု",
                ["category_growing_up"] = "ကြီးပြင်းလာခြင်း",
                ["category_healthy_relationships"] = "ကျန်းမာသော ဆက်ဆံရေးများ",
                ["category_friendships"] = "မိတ်ဆွေများ",
                ["category_family"] = "မိသားစု",
                ["category_emotions"] = "စိတ်ခံစားမှုများ",

                // Parent Information
                ["parent_information"] = "မိဘ/အုပ်ထိန်းသူ အချက်အလက် (ရွေးချယ်ရန်)",
                ["parent_name"] = "မိဘ/အုပ်ထိန်းသူ အမည်",
                ["parent_email"] = "မိဘ/အုပ်ထိန်းသူ အီးမေးလ်",
                ["email_notifications"] = "မိဘ အီးမေးလ်သို့ နေ့စဉ် သင်ယူမှု အစီရင်ခံစာများ ပို့ပေးရန်",
                ["send_daily_log"] = "📧 နေ့စဉ် လော့ဂ်ပို့ပေးရန်",
                ["sending_daily_log"] = "📧 မိဘ အီးမေးလ်သို့ နေ့စဉ် လော့ဂ်ပို့နေသည်...",
                ["daily_log_sent"] = "✅ မိဘ အီးမေးလ်သို့ နေ့စဉ် လော့ဂ်ပို့ပြီးပါပြီ!",
                ["daily_log_error"] = "❌ နေ့စဉ် လော့ဂ်ပို့ရာတွင် အမှား: {0}",

                // Messages
                ["typing_guidance"] = "မင်္ဂလာပါ! သင်ရိုက်နေသည်ကို ကျွန်တော် မြင်တွေ့ပါသည်၊ သို့သော် သင်ယူရန် အကောင်းဆုံးနည်းလမ်းမှာ အပေါ်ရှိ '📚 ခေါင်းစဉ်များ' ခလုတ်ကို နှိပ်ခြင်းဖြစ်သည်! ထိုနေရာတွင် သင်သည် ခေါင်းစဉ်များနှင့် မေးခွန်းများကို လေ့လာနိုင်သည်။ ရှိသော သင်ယူမှု ခေါင်းစဉ်များကို ကြည့်ရှုရန် ထိုခလုတ်ကို နှိပ်ကြည့်ပါ! 😊",
                ["back_to_start_confirm"] = "သင်သည် စတင်ရန် စာမျက်နှာသို့ ပြန်သွားရန် သေချာပါသလား?",
                ["back_to_start_title"] = "စတင်ရန် ပြန်သွားရန်",

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
