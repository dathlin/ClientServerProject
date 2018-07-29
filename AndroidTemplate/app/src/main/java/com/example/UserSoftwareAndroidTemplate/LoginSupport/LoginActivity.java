package com.example.UserSoftwareAndroidTemplate.LoginSupport;

import android.animation.Animator;
import android.animation.AnimatorSet;
import android.animation.ObjectAnimator;
import android.animation.PropertyValuesHolder;
import android.animation.ValueAnimator;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Handler;
import android.os.Message;
import android.provider.Settings;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.telephony.TelephonyManager;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.animation.AccelerateDecelerateInterpolator;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;


import com.example.UserSoftwareAndroidTemplate.CommonLibrary.SimplifyHeadCode;
import com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount;
import com.example.UserSoftwareAndroidTemplate.LogUtil;
import com.example.UserSoftwareAndroidTemplate.MainActivity;
import com.example.UserSoftwareAndroidTemplate.R;
import com.example.UserSoftwareAndroidTemplate.UserClient;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;

import org.json.JSONObject;

import java.util.Date;

import HslCommunication.BasicFramework.SystemVersion;
import HslCommunication.Core.Net.NetHandle;
import HslCommunication.Core.Types.OperateResultExOne;

import static com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount.FrameworkVersion;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.activity_login);
        initView();


        EditText editText1 = (EditText) findViewById(R.id.login_intput_username);

        SharedPreferences pref = getSharedPreferences(UserClient.SettingsFileName,MODE_PRIVATE);


        String name = pref.getString("name","");
        if("".equals(name))
        {

        }
        else {

            editText1.setText(name);
            EditText editText2 = (EditText) findViewById(R.id.login_intput_password);
            editText2.setFocusable(true);
        }
    }




    private TextView mBtnLogin;
    private View progress;
    private View mInputLayout;
    private float mWidth, mHeight;
    private LinearLayout mName, mPsw;
    /*
        系统的登录状态，0，未登录，1，登入中
     */
    private int logStatus = 0;

    private void initView() {
        mBtnLogin = (TextView) findViewById(R.id.main_btn_login);
        progress = findViewById(R.id.layout_progress);
        mInputLayout = findViewById(R.id.input_layout);
        mName = (LinearLayout) findViewById(R.id.input_layout_name);
        mPsw = (LinearLayout) findViewById(R.id.input_layout_psw);

        mBtnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(logStatus==0) {

                    // 应该在这里启动账户验证
                    EditText editText1 = (EditText) findViewById(R.id.login_intput_username);
                    UserName = editText1.getText().toString();
                    if("".equals(UserName))
                    {
                        Toast.makeText(LoginActivity.this, "请输入用户名", Toast.LENGTH_SHORT).show();
                        editText1.setFocusable(true);
                        return;
                    }


                    EditText editText2 = (EditText) findViewById(R.id.login_intput_password);
                    UserPassword = editText2.getText().toString();
                    if("".equals(UserPassword))
                    {
                        Toast.makeText(LoginActivity.this, "请输入密码", Toast.LENGTH_SHORT).show();
                        editText2.setFocusable(true);
                        return;
                    }


                    // 开启程序验证
                    logStatus = 1;

                    // 计算出控件的高与宽
                    mWidth = mBtnLogin.getMeasuredWidth(); // 209

                    LogUtil.LogD("onClick",""+mWidth);

                    mHeight = mBtnLogin.getMeasuredHeight();

                    // 隐藏输入框
                    mName.setVisibility(View.INVISIBLE);
                    mPsw.setVisibility(View.INVISIBLE);

                    inputAnimator(mInputLayout, mWidth, mHeight);


                }
            }
        });
    }


    private void InterfaceRestore()
    {
        logStatus = 0;
        // 重新显示界面
        progress.setVisibility(View.INVISIBLE);


        //mInputLayout.SCALE_X=1f;
        mInputLayout.setScaleX(1);
        ViewGroup.MarginLayoutParams params = (ViewGroup.MarginLayoutParams)mInputLayout.getLayoutParams();
        params.leftMargin=0;
        params.rightMargin=0;
        mInputLayout.setLayoutParams(params);
        mInputLayout.setVisibility(View.VISIBLE);


        mName.setVisibility(View.VISIBLE);
        mPsw.setVisibility(View.VISIBLE);
    }


    private Handler handler=new Handler(){
        /**重写handleMessage方法*/
        @Override
        public void handleMessage(Message msg) {
            if (msg.arg1 == 1) {
                // 登录失败并且显示信息
                Toast.makeText(LoginActivity.this, msg.obj.toString(), Toast.LENGTH_SHORT).show();
                InterfaceRestore();
            }
            else if(msg.arg1 == 2)
            {
                // 登录成功
                Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                startActivity(intent);
                finish();
                //两个参数分别表示进入的动画,退出的动画
                overridePendingTransition(R.anim.fade_in, R.anim.fade_out);

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


    private String UserName = "";
    private String UserPassword = "";

    /**
     * 网络操作相关的子线程
     */
    Runnable loginSystem = new Runnable() {
        @Override
        public void run() {

            long startTime = System.currentTimeMillis();

            LogUtil.LogD("loginSystem","开始请求维护检查");
            // 第一步请求维护状态
            OperateResultExOne<String> result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.维护检查, "");
            if (!result.IsSuccess) {
                MessageShow(result.ToMessageShowString());
                return;
            }

            if (!result.Content.equals("1")) {
                MessageShow(result.Content.substring(1));
                return;
            }

            LogUtil.LogD("loginSystem","开始请求账户检查");
            // 第二步检查账户
            JsonObject jsonObject = new JsonObject();
            jsonObject.addProperty(UserAccount.UserNameText, UserName);
            jsonObject.addProperty(UserAccount.PasswordText, UserPassword);
            jsonObject.addProperty(UserAccount.LoginWayText, "Andriod");
            jsonObject.addProperty(UserAccount.DeviceUniqueID, "Missing");
            jsonObject.addProperty(FrameworkVersion, UserClient.FrameworkVersion.toString());

            LogUtil.LogD("loginSystem",jsonObject.toString());
            result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.账户检查, jsonObject.toString());
            if (!result.IsSuccess) {
                MessageShow(result.ToMessageShowString());
                return;
            }

            UserAccount account = new Gson().fromJson(result.Content, UserAccount.class);
            if (!account.LoginEnable) {
                // 不允许登录
                MessageShow(account.ForbidMessage);
                return;
            }
            UserClient.UserAccount = account;
            // 保存用户名和密码
            SharedPreferences.Editor editor = getSharedPreferences(UserClient.SettingsFileName,MODE_PRIVATE).edit();
            editor.putString("name",UserName);
            editor.putString("password",UserPassword);
            editor.putLong("time",System.currentTimeMillis());
            editor.apply();


            LogUtil.LogD("loginSystem","开始请求版本检查");
            // 第三步检查版本
            result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.更新检查, "");
            if (!result.IsSuccess) {
                MessageShow(result.ToMessageShowString());
                return;
            }

            SystemVersion sv = new SystemVersion(result.Content);

            if (account.UserName != "admin") {
                if (!UserClient.CurrentVersion.IsSameVersion(sv)) {
                    MessageShow("版本号校验失败！服务器版本为：" + sv.toString());

                    // 此处应该启动下载更新，这部分的服务以后再完成
                    return;
                }
            } else {
                if(UserClient.CurrentVersion.IsSmallerThan(sv))
                {
                    // 管理员也不允许以低版本登录
                    MessageShow("版本号校验失败！服务器版本为：" + sv.toString());
                    return;
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
                return;
            }

            long loadingTime = System.currentTimeMillis() - startTime;
            // 如果时间很短，做个延时处理
            if (loadingTime < 2000) {
                try {
                    Thread.sleep(2000 - loadingTime);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }

            LogUtil.LogD("loginSystem","登录成功");
            // 登录成功
            Message msg = new Message();
            msg.arg1 = 2;
            handler.sendMessage(msg);
        }
    };

    /**
     * 输入框的动画效果
     *
     * @param view
     *      控件
     * @param w
     *      宽
     * @param h
     *      高
     */
    private void inputAnimator(final View view, float w, float h) {

        AnimatorSet set = new AnimatorSet();

        ValueAnimator animator = ValueAnimator.ofFloat(0, w);
        animator.addUpdateListener(new ValueAnimator.AnimatorUpdateListener() {

            @Override
            public void onAnimationUpdate(ValueAnimator animation) {
                float value = (Float) animation.getAnimatedValue();
                ViewGroup.MarginLayoutParams params = (ViewGroup.MarginLayoutParams) view
                        .getLayoutParams();
                params.leftMargin = (int) value;
                params.rightMargin = (int) value;

                view.setLayoutParams(params);
            }
        });

        ObjectAnimator animator2 = ObjectAnimator.ofFloat(mInputLayout,
                "scaleX", 1f, 0.5f);
        set.setDuration(300);
        set.setInterpolator(new AccelerateDecelerateInterpolator());
        set.playTogether(animator, animator2);
        set.start();
        set.addListener(new Animator.AnimatorListener() {

            @Override
            public void onAnimationStart(Animator animation) {

            }

            @Override
            public void onAnimationRepeat(Animator animation) {

            }

            @Override
            public void onAnimationEnd(Animator animation) {
                /**
                 * 动画结束后，先显示加载的动画，然后再隐藏输入框
                 */
                progress.setVisibility(View.VISIBLE);
                progressAnimator(progress);
                mInputLayout.setVisibility(View.INVISIBLE);


                /*
                    启动后台验证程序
                 */
                new Thread(loginSystem).start();
            }

            @Override
            public void onAnimationCancel(Animator animation) {

            }
        });

    }

    /**
     * 出现进度动画
     *
     * @param view
     */
    private void progressAnimator(final View view) {
        PropertyValuesHolder animator = PropertyValuesHolder.ofFloat("scaleX",
                0.5f, 1f);
        PropertyValuesHolder animator2 = PropertyValuesHolder.ofFloat("scaleY",
                0.5f, 1f);
        ObjectAnimator animator3 = ObjectAnimator.ofPropertyValuesHolder(view,
                animator, animator2);
        animator3.setDuration(1000);
        animator3.setInterpolator(new JellyInterpolator());
        animator3.start();

    }
}
