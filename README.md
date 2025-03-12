# SoundCatch
2024년 서울여자대학교 소프트웨어융합학과 프로젝트종합설계 졸업 프로젝트<br/>

3D 핸드트래킹 PC 게임<br/>
시각장애인과 비시각장애인 모두 즐길 수 있는 소리와 핸드트래킹 기술을 이용한 게임<br/>
* 본 프로젝트는 서울여자대학교 소프트웨어융합학과 졸업 프로젝트 결과물입니다.

게임 플레이 영상
<br/>https://www.youtube.com/watch?v=lwgDjd2JFuY

## 게임 소개
'SoundCatch'는 별도 보조기기 및 프로그램을 사용하지 않아도 시각장애인이 비시각장애인과 가능한 동일한 플레이를 경험할 수 있는 게임입니다.<br>
핸드트래킹 기술과 오디오 중심 플레이를 제공하여 게임 자체 접근성을 향상하고, 시각장애인과 비시각장애인의 문화 격차 해소, 공감대 형성을 프로젝트 목적으로 하고 있습니다. <br/>
### 게임 특징
* 핸드트래킹을 통해 직접 손을 움직여 조작하는 게임 방식
  * 직접 손을 움직여 메뉴를 선택하고 게임을 플레이 할 수 있습니다.
  <br/><img width="30%" src="https://github.com/user-attachments/assets/bb5be267-fe35-41ad-bcac-33259ce29eba"/><img width="30%" src="https://github.com/user-attachments/assets/f344823f-c49a-401d-8a8b-b91142275dcc"/><img width="30%" src="https://github.com/user-attachments/assets/19609e2b-1d00-4b63-a751-d0bc0cb302aa"/>
  <br/><img width="50%" src="https://github.com/user-attachments/assets/7b6e397d-de21-48ee-9efb-05eeb0794972"/><img width="50%" src="https://github.com/user-attachments/assets/2d37a6f0-645a-4972-9648-61273c3dd9cd"/>
* 세 가지의 게임
  *  숨은 소리 찾기, 음 맞추기, 손동작 외우기 게임을 각 세 가지의 난이도로 제공합니다.
  <br/><img width="50%" src="https://github.com/user-attachments/assets/3ab24544-d387-4a8f-9f98-aeb9117b5b13"/><img width="50%" src="https://github.com/user-attachments/assets/12258a3f-8f93-4be1-bc8d-914733d25e6d"/>
* 청각과 시각을 통한 사용자 인터페이스 제공
  * 청각을 통한 인터페이스는 물론, 색각이상자도 구별 가능한 컬러시스템을 사용하여 시각을 통한 인터페이스를 제공합니다.
  * 모든 메뉴를 음성으로 제공하고 있으며, 화면 이동 시 화면에 대한 묘사도 음성으로 제공합니다.
  <br/><img width="80%" src="https://github.com/user-attachments/assets/5e52731a-4dd5-4940-8680-629f90f7ad33"/>
* 한국디지털접근성진흥원 방문 인터뷰 진행
  * 프로토타입 단계에서 프로젝트 전반에 대한 피드백을 받기 위해 한국디지털접근성진흥원에 방문하여 인터뷰를 진행했습니다.
## 프로젝트 개요
🔗자세한 내용은 Notion에서 확인하실 수 있으십니다.    [<img src="https://img.shields.io/badge/Notion-000000?style=flat-round&logo=Notion&logoColor=white"/>](https://www.notion.so/SoundCatch-178b66b96b7780e2b49fe8e487baab2b?pvs=4)
### 개발 기간
* 2024.03 - 2024.12 (약 9개월)
### 개발 환경
* Unity 2022.3.11f1
* Python CVZone, OpenCV, Mediapipe
* Teachable Machine
### 수행업무
프로젝트 팀원은 3명으로 그 중 개발에 참여하여 다음과 같은 부분을 담당하였습니다.<br/>

핸드트래킹 손 인식 및 손 동작 구별 모델 제작
* Google Teachable Machine을 사용하여 세 가지 손 동작을 약 4000장 학습시켜 손 동작 구별 모델 제작
* Python의 Mediapipe, OpenCV, CVZone 라이브러리를 사용하여 카메라를 통해 1초에 10번 손 위치 인식 제작
* 손의 랜드마크 사이의 거리를 통해 카메라와 손 사이의 거리를 측정하여 게임에서 사용하는 손 이외의 손 인식 방지

손 동작 및 손 위치 통신
* UDP 소켓 통신을 통해 Python에서 손 위치, 손 동작 구별값, 손 인식 제한 값 등 게임에 필요한 값들을 Unity로 통신

손 동작 및 손 위치 조작 제작
* Diagnostics 네임스페이스를 사용하여 손 인식 및 손 동작 구별 python .exe 파일 실행
  * ProcessStartInfo를 사용하여 python .exe 실행 파일을 실행하여 Unity에서 Python 실행 파일을 실행할 수 있도록 설정
* Net, Net.Socket, Threading 네임스페이스를 사용하여 Python에서 보낸 UDP 소켓 통신 수신\
  * Thread를 사용하여 Unity 게임이 실행되면서 동시에 Python 실행 파일로 부터 손 관련 데이터를 받아오도록 설정
* 손 위치에 Raycast를 통한 오브젝트 인식으로 게임 조작 제작

오브젝트 선택 및 이벤트 호출, 사운드 처리 자동화 내부 툴 개발
* HandTracking 씬에서 Raycast를 통해 오브젝트(UI, 게임 오브젝트)를 감지 및 구분, 이에 따른 사운드 출력 및 이벤트 호출을 자동으로 처리하는 시스템 구축
  * ScriptableObject를 이벤트 채널로 활용하여, 이벤트 정의는 각 씬에서,  호출은 HandTracking 씬에서 중앙 집중식으로 관리하고 실행하도록 제작
  * 각 씬에서 정의된 이벤트를 HandTracking 씬에서 호출하도록 하여, 개별 오브젝트와 연결할 필요 없이 중앙에서 이벤트 실행을 관리
  * 오브젝트의 Inspector 창에 오브젝트 정보 스크립트를 추가하고, 이벤트를 정의한 후 이를 ScriptableObject 이벤트 채널로 연결하면, 자동으로 사운드 및 이벤트 처리가 이루어지도록 제작
* 이벤트 정의 및 호출 과정 자동화, 일괄 관리를 통한 반복 작업 감소로 작업 시간 절약 및 효율성 향상

손 동작 외우기 게임 제작
* Coroutine을 활용하여 손동작을 음성으로 알려주고 3초 안에 알맞은 손동작을 취해야 하는 게임 제작

키보드 조작 추가
* Input을 활용해 키보드를 활용한 조작 제작

게임 난이도 선택 씬 추가
* Switch 문을 사용한 게임 종류에 따른 씬 이동 구분
## 프로젝트 성과
* 서울여자대학교 소프트웨어융합학과 2024-2 졸업전시회 참여
* 2024학년도 한국스마트미디어학회 & 한국전자거래학회 추계학술대회 우수논문상 수상
