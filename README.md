**Project Codename: Apollo**

Description: Car renting platform. Providers can register vehicles of various types. Customer can search for available vehicles according to their current location and booking.

Team 01
Students:
1) Boluwatife Adebola Alawode, 7219092, boluwatife.alawode001@stud.fh-dortmund.de
2) Mustafa Ali, 7218952, mustafa.ali003@stud.fh-dortmund.de

Database:
https://github.com/bjdavey/Apollo.DB
MySQL v8.4.0

Backend:
https://github.com/bjdavey/Apollo.API
.NET Core 8.0.6 Web API
https://apollo-api.bjdavey.online/


Frontend:
https://github.com/bjdavey/Apollo.Web
Angular +2 with DevExtreme
https://apollo.bjdavey.online
email-admin: admin@apollo.tech
email-provider: provider1@apollo.tech
email-customer: customer1@apollo.tech
password (all): 0000


-------------------------------
Traccar (Open Source Software):
https://traccar.bjdavey.online or http://185.222.241.148:8082/
email: jbjdavey@hotmail.com
password: 0000

You can download Traccar Client Mobile app from https://www.traccar.org/client/
1) Set the "Device identifier" to the "Device Unique" of the vehicle that you created in Apollo
2) Set the Server URL: https://traccar-client.bjdavey.online or http://185.222.241.148:5055
3) Switch on and check the status


--------------------------------------------------------------
If you wish to deploy the project in your environment:
1) You need to install Traccar server (https://www.traccar.org/download/)
2) You need MySQL v8.4.0, build the database using this sql script https://github.com/bjdavey/Apollo.DB
2) Clone, edit appsettings.json, and run the backend https://github.com/bjdavey/Apollo.API (.NET Core 8.0.6 Web API)
3) Clone the frontend https://github.com/bjdavey/Apollo.Web, edit environment.local.ts to point the "apiUrl" to your backend, then run using the "start-local" script in package.json
