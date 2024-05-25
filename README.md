# Im Not Playing

해당 프로젝트는 한 기업의 업무효율화를 명목으로 직원들의 마우스 포인터 움직임 감시를 통해 불합리한 방식으로 업무시간을 측정하고 있다는 기사를 보고 시작되었습니다.

웹캠과 OpenCV, ML.NET을 기반으로 얼굴인식, 얼굴분별 기능이 있으며, Postgres DB에 데이터를 지속적으로 저장 및 관리할 수 있도록 하였습니다.

해당 기능은 아래와 같습니다.

1. 웹캠을 통한 사용자 얼굴 인식
2. 기존의 학습된 얼굴과 해당 얼굴 분별
3. 일시정지/재시작 기능을 통한 업무 외적인 시간에 대한 예외시간 관리
4. 정확한 업무시간의 지속적인 Data 관리


   ![image](https://github.com/hyunpap2r/Im-Not-Playing/assets/91259577/7c1141c2-9ffd-4bf4-ac62-3b5191ebb081)


   ![image](https://github.com/hyunpap2r/Im-Not-Playing/assets/91259577/a207c3e6-705a-4d44-b9fd-6a748b4cfc92)





현재는 위와 같은 기능을만 추가되어 있으며, 이 기능들을 기반으로 하여 다양한 기능들을 지속적으로 추가할 예정입니다.

추가 예정 기능
1. Google Assistant API 연동을 통한 제어.
2. Google Assistant를 통한 통합제어 프로그램 개발.
3. 승인된 얼굴인식 상태에서만 특정 문서 및 프로그램 접근/제어 허용.


최적화 예정

1. Connection Pool을 통한 DB 연결 효율성 관리.
2. ML사용시 메모리 사용량 최적화.
3. 코드 리펙토링.
