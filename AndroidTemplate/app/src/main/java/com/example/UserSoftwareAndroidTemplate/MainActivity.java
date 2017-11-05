package com.example.UserSoftwareAndroidTemplate;

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

import com.example.HslCommunication.Core.Types.NetHandle;
import com.example.HslCommunication.Core.Types.OperateResultString;
import com.example.HslCommunication.Enthernet.NetSimplifyClient;

import java.util.UUID;

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
                OperateResultString result = (OperateResultString) msg.obj;
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
            OperateResultString result = UserClient.Client.ReadFromServer(new NetHandle(1,2, 15),"",null,null);
            msg.arg1=1;
            msg.obj = result;
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

        TextView textView = (TextView) findViewById(R.id.textViewMain);
        textView.setMovementMethod(new ScrollingMovementMethod());
    }
}
