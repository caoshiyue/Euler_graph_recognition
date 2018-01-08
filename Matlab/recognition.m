function adjacent_m=recognition(InPathArray)
clf;
input = imread('2.jpg');
info=imfinfo('2.jpg');
input_m=info.Height;input_n=info.Width;
input_gray = rgb2gray(input);
input_gray = filter2(fspecial('average',3),input_gray);
input_bw = edge(input_gray);
%---------------------------------------------------图像无边框处理
 input_bw(1:3,1:input_n)=0;
 input_bw(input_m-3:input_m,1:input_n)=0;
 input_bw(1:input_m,1:3)=0;
 input_bw(1:input_m,input_n-3:input_n)=0;
%--------------------------------------------------
% imshow(input_bw)
input_bw_uint8 = zeros(input_m,input_n);
input_bw_uint8(input_bw == 1) = 0;
input_bw_uint8(input_bw == 0) = 255;

se1=strel('disk',2);
input_bw_min=imerode(input_bw_uint8,se1);
%input_bw_min=input_bw_uint8;
% figure
% imshow(input_bw_min);
%% 去除圆
[centers, radii] = imfindcircles(input_bw_min,[7 14],'ObjectPolarity','dark','Sensitivity',0.92,'Method','TwoStage');
len=length(radii);
for n=1:len
C(n).ymin=round(centers(n+len))-round(radii(n)+30);
C(n).ymax=round(centers(n+len))+round(radii(n)+30);
C(n).xmin=round(centers(n))-round(radii(n)+30);
C(n).xmax=round(centers(n))+round(radii(n)+30);
if C(n).xmin<1
    C(n).xmin=1;
end
if C(n).ymin<1
    C(n).ymin=1;
end
if C(n).ymax>input_m
    C(n).ymax=input_m;
end
if C(n).xmax>input_n
    C(n).xmax=input_n;
end
% input_bw_min(C(n).ymin+3:C(n).ymax-3,C(n).xmin+3:C(n).xmax-3)=1;

for yn_transfer= C(n).ymin:C(n).ymax,
    for xn_transfer=C(n).xmin:C(n).xmax,
        d_transfer=sqrt(power(xn_transfer-centers(n),2)+power(yn_transfer-centers(n+len),2));
        if d_transfer<radii(n)+25,
            input_bw_min(yn_transfer,xn_transfer)=1;
        end
    end
end

end

%% 连通区域检测
 [L, num] = bwlabel(~input_bw_min,8);
 Prearray=zeros(len,10);
 for m=1:len
    Pointlist=[];
    for ym_detect= C(m).ymin:C(m).ymax,
        for xm_detect=C(m).xmin:C(m).xmax,
            d_detect=sqrt(power(xm_detect-centers(m),2)+power(ym_detect-centers(m+len),2));
            if d_detect<radii(m)+28 && d_detect>radii(m)+25,
                %------------
                Pointlist=[Pointlist L(ym_detect,xm_detect)];
                Pointlist=unique(Pointlist);
                len_=length(Pointlist);
                for line=1:len_,
                    Prearray(m,line)=Pointlist(line);
                end
            end
        end
    end
 end
 
 %% 生成邻接矩阵
 %Prearray(:,1)=[];
 Array=zeros(len,len);

 for i=1:len,
     for j=1:10,
         if Prearray(i,j)~=0,
         flag=0;
             for x=1:len,
                 for y=1:10,
                     if Prearray(i,j)==Prearray(x,y)
                         if i~=x,
                         Array(i,x)=Array(i,x)+1;
                         flag=1;
                         %disp('output');
                         end
                     end
                 end
             end
             if flag==0,
                Array(i,i)=1;
             end
         end
     end
 end

%% 输出
 imshow(input,'border','tight','initialmagnification','fit');
set(gcf,'NumberTitle', 'off','name','recognition','MenuBar','none','color','white');
for label=1:len,
    text(centers(label)+5,centers(label+len)+5,num2str(label-1),'Color','b','FontSize',20);
end
adjacent_m=Array;
end


