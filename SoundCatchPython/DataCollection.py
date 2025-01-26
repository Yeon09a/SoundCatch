from cv2 import VideoCapture, resize, INTER_CUBIC, CAP_PROP_FRAME_WIDTH, CAP_PROP_FRAME_HEIGHT, imshow, waitKey, imwrite, destroyAllWindows
from cvzone.HandTrackingModule import HandDetector
from cvzone.FaceMeshModule import FaceMeshDetector
from cvzone import putTextRect
import socket
import numpy as np
from math import ceil, sqrt
import sys
from os.path import join
from cvzone.ClassificationModule import Classifier
import time

width, height = 1280, 720

cap =  VideoCapture(0)
cap.set(3, width)
cap.set(4, height)
detector = HandDetector(maxHands= 2, detectionCon=0.8)
detector_face = FaceMeshDetector(maxFaces=1) # FaceDetector

offset = 20
imgSize = 300

folder = "ImageData/Scissor"
counter = 0

while True:
    success, img = cap.read()
    
    # 1280x720을 지원하지 않는 해상도 지원용 처리
    if not (int(cap.get(CAP_PROP_FRAME_WIDTH)) == width) & (int(cap.get(CAP_PROP_FRAME_HEIGHT)) == height):
        img2 = resize(img, (width, height), interpolation=INTER_CUBIC)
        img = img2
    
    if not success:
            break
        
    img3, faces = detector_face.findFaceMesh(img, draw=False)

    d = 2
    
    if faces:
        face = faces[0]
        pointLeft = face[145]
        pointRight = face[374]
        w, _ = detector_face.findDistance(pointLeft, pointRight)
        W = 6.3

        # Finding distancesssssssssssssssssssssssssss
        f = 840
        d = (W * f) / w
        #print(d)   
        
    if d >= 95:
        dis = 0
    elif d > 60:
        dis = 1
        radius = 3
    elif 35:
        dis = 1
        radius = 5
    else :
        dis = 2
        
        
    hands, img = detector.findHands(img, radius)
    
    
    
    if hands:
        hand = hands[0]
        x, y, w, h = hand["bbox"]
        
        
        imgWhite = np.ones((imgSize, imgSize, 3), np.uint8) * 255
        imgCrop = img[y-offset:y+h+offset, x-offset:x+w+offset]
        
        imgCropShape = imgCrop.shape
        
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
                
                imshow("ImageCrop", imgCrop)
                imshow("ImageWhite", imgWhite)
                
        except Exception as e:
            print(str(e))
            
    imshow("Image", img)
    key = waitKey(1)
    if key == ord("s"):
        counter += 1
        imwrite(f'{folder}/Image_{time.time()}.png', imgWhite)
        print(counter)