package com.example.UserSoftwareAndroidTemplate.LoginSupport;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.widget.TextView;
import android.widget.Toast;

import com.example.UserSoftwareAndroidTemplate.CommonLibrary.SimplifyHeadCode;
import com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount;
import com.example.UserSoftwareAndroidTemplate.LogUtil;
import com.example.UserSoftwareAndroidTemplate.MainActivity;
import com.example.UserSoftwareAndroidTemplate.R;
import com.example.UserSoftwareAndroidTemplate.UserClient;
import com.google.gson.Gson;
import com.google.gson.JsonObject;

import HslCommunication.BasicFramework.SystemVersion;
import HslCommunication.Core.Types.OperateResultExOne;

import static com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount.FrameworkVersion;


public class SplashActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        TextView mVersionNameText = (TextView) findViewById(R.id.TextVersionInfo);
        mVersionNameText.setText("Version: " + UserClient.CurrentVersion.toString());

        new AsyncTask<Void, Void, Integer>() {


            @Override
            protected Integer doInBackground(Void... voids) {
                int result;
                long startTime = System.currentTimeMillis();
                result = loadingCache();
                long loadingTime = System.currentTimeMillis() - startTime;
                if (loadingTime < SHOW_TIME_MIN) {
                    try {
                        Thread.sleep(SHOW_TIME_MIN - loadingTime);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                return result;
            }

            @Override
            protected void onPostExecute(Integer result) {
                if (result > 0) {
                    if (result == SUCCESSTWO) {
                        Intent intent = new Intent(SplashActivity.this, LoginActivity.class);
                        startActivity(intent);
                        finish();
                        //两个参数分别表示进入的动画,退出的动画
                        overridePendingTransition(R.anim.fade_in, R.anim.fade_out);
                    } else {
                        Intent intent = new Intent(SplashActivity.this, MainActivity.class);
                        startActivity(intent);
                        finish();
                        //两个参数分别表示进入的动画,退出的动画
                        overridePendingTransition(R.anim.fade_in, R.anim.fade_out);
                    }
                }
            }

            ;
        }.execute(new Void[]{});

    }

    private static final int SHOW_TIME_MIN = 1200;
    private static final int FAILURE = 0;            // 失败
    private static final int SUCCESSONE = 1;         // 直接登录
    private static final int SUCCESSTWO = 2;         // 显示登录窗口


    private Handler handler=new Handler(){
        /**重写handleMessage方法*/
        @Override
        public void handleMessage(Message msg) {
            if (msg.arg1 == 1) {
                Toast.makeText(SplashActivity.this, msg.obj.toString(), Toast.LENGTH_SHORT).show();
            }
        }
    };


    private void MessageShow(String message)
    {
        Message msg = new Message();
        msg.arg1=1;
        msg.obj = message;
        handler.sendMessage(msg);
    }


    private int loadingCache() {

        // 检查登录状态
        SharedPreferences pref = getSharedPreferences(UserClient.SettingsFileName,MODE_PRIVATE);
        if(pref == null) {
            return SUCCESSTWO;
        }


        String name = pref.getString("name","");
        String password = pref.getString("password","");
        long time = pref.getLong("time",0);


        if("".equals(name))
        {
            return SUCCESSTWO;
        }

        if("".equals(password))
        {
            return SUCCESSTWO;
        }

        if((System.currentTimeMillis()-time)/1000/60/60/24>6)
        {
            // 七天未登录就放弃
            return SUCCESSTWO;
        }


        // 直接登录
        LogUtil.LogD("loginSystem","开始请求维护检查");


        // 第一步请求维护状态
        OperateResultExOne<String> result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.维护检查, "");
        if (!result.IsSuccess) {
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }

        if (!result.Content.equals("1")) {
            MessageShow(result.Content.substring(1));
            return FAILURE;
        }


        if(!result.Content.equals("1")) {
            MessageShow(result.Content.substring(1));
            return FAILURE;
        }


        LogUtil.LogD("loginSystem","开始请求账户检查");
        // 第二步检查账户
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty(UserAccount.UserNameText, name);
        jsonObject.addProperty(UserAccount.PasswordText, password);
        jsonObject.addProperty(UserAccount.LoginWayText, "Andriod");
        jsonObject.addProperty(UserAccount.DeviceUniqueID, "Missing");
        jsonObject.addProperty(FrameworkVersion, UserClient.FrameworkVersion.toString());

        LogUtil.LogD("loginSystem",jsonObject.toString());
        result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.账户检查, jsonObject.toString());
        if (!result.IsSuccess) {
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }

        UserAccount account = new Gson().fromJson(result.Content, UserAccount.class);
        if (!account.LoginEnable) {
            // 不允许登录
            MessageShow(account.ForbidMessage);
            return SUCCESSTWO;
        }
        UserClient.UserAccount = account;


        LogUtil.LogD("loginSystem","开始请求版本检查");
        // 第三步检查版本
        result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.更新检查, "");
        if (!result.IsSuccess) {
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }

        SystemVersion sv = new SystemVersion(result.Content);

        if (account.UserName != "admin") {
            if (!UserClient.CurrentVersion.IsSameVersion(sv)) {
                MessageShow("版本号校验失败！服务器版本为：" + sv.toString());

                // 此处应该启动下载更新，这部分的服务以后再完成
                return FAILURE;
            }
        } else {
            if(UserClient.CurrentVersion.IsSmallerThan(sv))
            {
                // 管理员也不允许以低版本登录
                MessageShow("版本号校验失败！服务器版本为：" + sv.toString());
                return FAILURE;
            }
        }


        LogUtil.LogD("loginSystem","开始请求数据下载");
        // 下载服务器数据
        result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.参数下载,"");
        if (result.IsSuccess) {
            // 服务器返回初始化的数据，此处进行数据的提取，有可能包含了多个数据
            JsonBeanPara para = new Gson().fromJson(result.Content, JsonBeanPara.class);
            // 例如公告数据和分厂数据
            UserClient.Announcement = para.Announcement;
            UserClient.SystemFactories = para.SystemFactories;
        }
        else
        {
            // 访问失败
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }


        LogUtil.LogD("loginSystem","登录成功");



        return SUCCESSONE;
    }

}
