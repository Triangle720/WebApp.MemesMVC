# Memes World
Kod aplikacji webowej w architekturze MVC tworzony w ramach portfolio.  
Uruchomiona w serwisie [Azure](https://azure.com)  
Podłączona pod [API imgur'a](https://apidocs.imgur.com).  
Link: [Memes World](https://webappmemesmvc.azurewebsites.net) (OFF)  

# O aplikacji
Aplikacja pozwala na rejestrację (**bez weryfikacji poprzez email**) użytkownika w serwisie polegającym na przesyłaniu 'śmiesznych obrazków' czy też słodkich kotków, oraz przeglądaniu  ich.

Hasła użytkowników szyfrowane są przy użyciu algorytmu **MD5**.  

Po utworzeniu konta i zalogowaniu, użytkownik otrzymuje **Token JWT** służący do autoryzacji i uzyskuje możliwość do upload'u obrazów, które trafiają do **kontenera plików BLOB w chmurze Azure**.  

Akceptowanymi rozszerzeniami plików są: **.jpg, .jpeg, .png, .gif**.  

Przesłane obrazy muszą zostać **odrzucone lub zaakceptowane przez moderatora/administratora**.  
W przypadku zaakceptowania, plik zostaje wysyłany do serwisu [imgur](https://imgur.com), oraz będzie od tej pory dostępny dla każdego użytkownika na stronie głównej aplikacji.  
Jeśli natomiast obraz zostanie odrzucony, aplikacja usuwa go z chmury.  

Moderacja jest uprawniona do **banowania** użytkowników na określony czas, za łamanie regulaminu.  

Administracja posiada uprawnienia moderacji, oraz możliwość zmiany roli **użytkownik -> moderator** i odwrotnie.  

Ukarany użytkownik zostaje odblokowany w trakcie logowania po terminie bana.  
