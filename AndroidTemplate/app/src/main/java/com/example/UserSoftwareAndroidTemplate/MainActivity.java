package com.example.UserSoftwareAndroidTemplate;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.example.UserSoftwareAndroidTemplate.CommonLibrary.SimplifyHeadCode;
import com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount;
import com.example.UserSoftwareAndroidTemplate.LoginSupport.LoginActivity;
import com.example.UserSoftwareAndroidTemplate.LoginSupport.SplashActivity;
import com.google.gson.Gson;

import java.util.UUID;

import HslCommunication.Core.Net.NetHandle;
import HslCommunication.Core.Types.OperateResultExOne;

public class MainActivity extends AppCompatActivity {

    public MainActivity()
    {

    }



    private Handler handler=new Handler(){
        /**重写handleMessage方法*/
        @Override
        public void handleMessage(Message msg) {
            if (msg.arg1 == 1) {

                // 说明是一个网络的请求访问
                OperateResultExOne<String> result = (OperateResultExOne<String>) msg.obj;
                if (result.IsSuccess) {
                    TextView textView = (TextView) findViewById(R.id.textViewMain);
                    textView.setText(result.Content);
                } else {
                    Toast.makeText(MainActivity.this, result.ToMessageShowString(), Toast.LENGTH_LONG).show();
                }
            }
        }
    };


    /**
     * 网络操作相关的子线程
     */
    Runnable networkTask = new Runnable() {

        @Override
        public void run() {
            // 在这里进行网络请求相关操作
            Message msg = new Message();
                OperateResultExOne<String> result = UserClient.Net_simplify_client.ReadFromServer(SimplifyHeadCode.获取账户,"");

                msg.obj = result;
            msg.arg1=1;
            handler.sendMessage(msg);
        }
    };



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button button1 = (Button) findViewById(R.id.button);
        button1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new Thread(networkTask).start();
        }});

        Button button2 = (Button) findViewById(R.id.button2);
        button2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                TextView textView = (TextView) findViewById(R.id.textViewMain);
                textView.setText(new Gson().toJson(UserClient.UserAccount));
            }
        });


        Button button3 = (Button) findViewById(R.id.button3);
        button3.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                // 清除
                SharedPreferences.Editor editor = getSharedPreferences(UserClient.SettingsFileName,MODE_PRIVATE).edit();
                editor.putString("password","");
                editor.putLong("time",0);
                editor.apply();

                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                startActivity(intent);
                finish();
                //两个参数分别表示进入的动画,退出的动画
                overridePendingTransition(R.anim.fade_in, R.anim.fade_out);
            }
        });

        TextView textView = (TextView) findViewById(R.id.textViewMain);
        textView.setMovementMethod(new ScrollingMovementMethod());
    }
}
