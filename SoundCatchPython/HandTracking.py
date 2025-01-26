from cv2 import VideoCapture, resize, INTER_CUBIC, CAP_PROP_FRAME_WIDTH, CAP_PROP_FRAME_HEIGHT, imshow, waitKey, destroyAllWindows
from cvzone.HandTrackingModule import HandDetector
from cvzone.FaceMeshModule import FaceMeshDetector
from cvzone import putTextRect
import socket
import numpy as np
from math import ceil, sqrt
import sys
from os.path import join
from cvzone.ClassificationModule import Classifier

width, height = 1280, 720

cap =  VideoCapture(0)
cap.set(3, width)
cap.set(4, height)
detector = HandDetector(maxHands= 2, detectionCon=0.8)
detector_face = FaceMeshDetector(maxFaces=1) # FaceDetector

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)

base_path = getattr(sys, '_MEIPASS', '')
model_path = join(base_path, 'Model/keras_model.h5')
labels_path = join(base_path, 'Model/labels.txt')

classifier = Classifier(model_path, labels_path)

offset = 20
imgSize = 300

labels = ["Open", "Close", "Scissor"]

#Find Function
# x is the raw distance y is the value in cm
xf = [300, 245, 200, 170, 145, 130, 112, 103, 93, 87, 80, 75, 70, 67, 62, 59, 57]
yf = [20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100]

coff = np.polyfit(xf, yf, 2)

try:
    success, img = cap.read()
    sock.sendto(str.encode("true"), serverAddressPort)
    while True:
        success, img = cap.read()
        
        # 1280x720을 지원하지 않는 해상도 지원용 처리
        if not (int(cap.get(CAP_PROP_FRAME_WIDTH)) == width) & (int(cap.get(CAP_PROP_FRAME_HEIGHT)) == height):
            img2 = resize(img, (width, height), interpolation=INTER_CUBIC)
            img = img2
        
        if not success:
                break
            
        img3, faces = detector_face.findFaceMesh(img, draw=False)
        
        d = 60
        radius = 2
        dis = 1
        
        if faces:
            face = faces[0]
            pointLeft = face[145]
            pointRight = face[374]
            w, _ = detector_face.findDistance(pointLeft, pointRight)
            W = 6.3

            # Finding distance
            f = 840
            d = (W * f) / w
            
            if d >= 95:
                dis = 0
            elif d > 60:
                dis = 1
                radius = 3
            elif d > 35:
                dis = 1
                radius = 5
            else :
                dis = 2
        
        hands, img = detector.findHands(img, radius)
        
        data = []
        idx = 0
        
        if hands:
            hand = hands[0]
            x, y, w, h = hand["bbox"]
            
            lmList = hand["lmList"]
            x5, y5, z5 = lmList[5]
            x17, y17, z17 = lmList[17]
            
            distance = int(sqrt((y17 - y5)**2 + (x17 - x5)**2))
            A, B, C = coff
            distanceCM = A*distance**2 + B*distance + C
            
            
            if len(hands) >= 2:
                hand2 = hands[1]
                lmList2 = hand2["lmList"]
                x5, y5, z5 = lmList2[5]
                x17, y17, z17 = lmList2[17]
                distance2 = int(sqrt((y17 - y5)**2 + (x17 - x5)**2))
                distance2CM = A*distance2**2 + B*distance2 + C
                
                if distanceCM > distance2CM :
                    x, y, w, h = hand2["bbox"]
                    distanceCM = distance2CM
                    lmList = lmList2         
            
            
            imgWhite = np.ones((imgSize, imgSize, 3), np.uint8) * 255
            imgCrop = img[y-offset:y+h+offset, x-offset:x+w+offset]
            
            aspectRatio = h/w
            
            
            try:
                if success:
                    if aspectRatio > 1:
                        k = imgSize / h
                        wCal = ceil(k * w)
                        imgResize = resize(imgCrop, (wCal, imgSize))
                        imgResizeShape = imgResize.shape
                        wGap = ceil((imgSize - wCal) / 2)
                        imgWhite[:, wGap:wCal + wGap] = imgResize
                
                    else :
                        k = imgSize / w
                        hCal = ceil(k * h)
                        imgResize = resize(imgCrop, (imgSize, hCal))
                        imgResizeShape = imgResize.shape
                        hGap = ceil((imgSize - hCal) / 2)
                        imgWhite[hGap:hCal + hGap, :] = imgResize
                    
                    prediction, idx = classifier.getPrediction(imgWhite)
                    
                    putTextRect(imgWhite, f'{int(distanceCM)} cm', (50, 100))
                    
                    
                    for lm in lmList:
                        data.extend([lm[0], height - lm[1], lm[2]])
                    data.extend([idx])
                    data.extend([dis])
                    sock.sendto(str.encode(str(data)), serverAddressPort)
                    
            except Exception as e:
                #print(str(e))
                sock.sendto(str.encode("out"), serverAddressPort)
                
            imshow("Image", img)
            imshow("ImageWhite", imgWhite)
            imshow("imgResize", imgResize)
        
        
        waitKey(100)

except KeyboardInterrupt:
    cap.release()
    sock.close()
    destroyAllWindows()