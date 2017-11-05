package com.example.UserSoftwareAndroidTemplate.LoginSupport;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.example.HslCommunication.BasicFramework.SystemVersion;
import com.example.HslCommunication.Core.Types.NetHandle;
import com.example.HslCommunication.Core.Types.OperateResultString;
import com.example.UserSoftwareAndroidTemplate.CommonHeadCode.SimplifyHeadCode;
import com.example.UserSoftwareAndroidTemplate.MainActivity;
import com.example.UserSoftwareAndroidTemplate.R;
import com.example.UserSoftwareAndroidTemplate.UserClient;


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
                    Intent intent = new Intent(SplashActivity.this, MainActivity.class);
                    //intent.setClassName(SplashActivity.this, getString(R.string.));
                    startActivity(intent);
                    finish();
                    //两个参数分别表示进入的动画,退出的动画
                    overridePendingTransition(R.anim.fade_in, R.anim.fade_out);
                }
            }

            ;
        }.execute(new Void[]{});

    }

    private static final int SHOW_TIME_MIN = 3000;
    private static final int FAILURE = 0; // 失败
    private static final int SUCCESS = 1; // 成功
    private static final int OFFLINE = 2; // 如果支持离线阅读，进入离线模式


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

        // 第一步请求维护状态
        OperateResultString result=UserClient.Client.ReadFromServer(SimplifyHeadCode.维护检查,"",null,null);
        if(!result.IsSuccess){
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }

        if(!result.Content.equals("1")) {
            MessageShow(result.Content.substring(1));
            return FAILURE;
        }


        // 第二步检查账户






        // 第三步检查版本
        result=UserClient.Client.ReadFromServer(SimplifyHeadCode.维护检查,"",null,null);
        if(!result.IsSuccess){
            MessageShow(result.ToMessageShowString());
            return FAILURE;
        }

        SystemVersion sv=new SystemVersion(result.Content);
        if(!UserClient.CurrentVersion.IsSameVersion(sv)) return FAILURE;


        // 下载服务器数据


        return SUCCESS;
    }

}
