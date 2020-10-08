# Margarina Quest ?
salut dess,

1. fake en gros t'a besoin de te partir un [storage emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator#get-the-storage-emulator) 

2. pi t'a besoin aussi de [.netcore 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) pi [nodejs](https://nodejs.org/en/)!

3. tu part le dev server du client avec `npm start` dans le dossier `client`

4. pi le server tu peux le debug avec Visual Studio ou `dotnet run`

5. genre ca speux que ton client ait pas le bon url de ton backend, ajuste ca dans le fichier `client/config.json`

6. fais toi un user en callant le backend `POST /user` avec `username` et `password` dans le body en json (tu peux checker le code du controlleur si tes pas sur)

7. oh ouin aussi quand tu va runner ton backend, faudra que tu te fasse une table `Users` sinon ca va crasher

pi normalement toute marche ??? sinon post une issue et je vais te revenir dans les 15 a 20 jours ouvrables.