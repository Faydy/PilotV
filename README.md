# ✈️ Pilot Career Mod for GTA V (Script Hook V .NET)

## Despre Proiect 📜

Acest mod adaugă un sistem de carieră de pilot funcțional în Grand Theft Auto V, permițând jucătorilor să preia rolul unui pilot de linie. Poți efectua zboruri de pasageri sau de marfă între aeroporturile din San Andreas, folosind aeronave specifice, și câștiga bani pentru fiecare cursă finalizată cu succes.

Proiectul este dezvoltat folosind **Script Hook V .NET** (C#) și utilizează LemonUI pentru interfața meniului.

## 🚀 Caracteristici (V1.0)

* **Zboruri Carieră:** Acceptă job-uri de pilotaj direct de la punctele de preluare.
* **Tipuri de Zbor:** Alege între:
    * **Zboruri de Pasageri** (Ex: Luxor, Shamal, Jet)
    * **Zboruri de Marfă** (Ex: CargoPlane)
* **Rute Configurate:** Zboruri între Los Santos International Airport (LSIA) și Sandy Shores Airfield.
* **Navigație Simplă:** Sistem de blip-uri și markere pentru a indica aeronava generată și pista de aterizare la destinație.
* **Sistem de Plată:** Recompense în bani GTA la finalizarea misiunii.

## 📥 Instalare

Pentru a rula acest mod, trebuie să ai instalate următoarele dependențe esențiale:

### Cerințe Obligatorii

1.  **[Script Hook V](https://www.dev-c.com/gtav/scripthookv/)**: Instrumentul de bază pentru încărcarea de scripturi.
2.  **[Script Hook V .NET](https://github.com/crosire/scripthookvdotnet/releases)**: Permite rularea scripturilor C#.
3.  **[LemonUI](https://www.gta5-mods.com/tools/lemonui)**: Biblioteca necesară pentru afișarea meniurilor modului.

### Pași de Instalare

1.  Asigură-te că toate cerințele de mai sus sunt instalate corect în directorul principal al jocului GTA V.
2.  Descarcă ultima versiune a modului (`PilotV.dll` și `PilotV.pdb`) din secțiunea [Releases](LINK_CATRE_PAGINA_DE_RELEASES_DE_PE_GITHUB).
3.  Copiază fișierele descărcate (`PilotV.dll` și `PilotV.pdb`) în folderul `scripts/` din directorul jocului GTA V.

## 🎮 Cum se Utilizează

1.  **Localizează un Aeroport:** Mergi la Los Santos International Airport (LSIA) sau la Sandy Shores Airfield. Caută blip-urile verzi pe hartă.
2.  **Deschide Meniul:** Când te afli lângă markerul de job, apasă tasta de interacțiune **`E` (sau `INPUT_CONTEXT`)** pentru a deschide meniul principal.
3.  **Configurează Zborul:**
    * Alege **Flight Type** (Passenger sau Cargo).
    * Alege **Destination** (destinația opusă celei curente).
    * Alege **Available Planes** (avionul dorit).
4.  **Începe Misiunea:** Apasă **Continue**. Jucătorul va fi teleportat în avion, iar misiunea începe.
5.  **Finalizare:** Zboară și aterizează la destinație pentru a primi plata.

## 🐞 Raportarea Problemelor

Dacă găsești erori (bug-uri) sau ai sugestii, te rog să deschizi un [Issue nou pe GitHub](LINK_CATRE_PAGINA_DE_ISSUES_DE_PE_GITHUB).

## 💡 Planuri de Viitor

* Adăugarea mai multor aeroporturi (ex: Fort Zancudo, Grapeseed).
* Introducerea avioanelor militare.
* Sistem de *reputație* sau *licențe* pentru a debloca avioane mai mari/misiuni mai bine plătite.
* Verificare mai strictă a aterizării (viteza).

---

## Licență

Acest proiect este licențiat sub [Licența MIT](LINK_CATRE_FISIERUL_LICENSE_DE_PE_GITHUB) - vezi fișierul `LICENSE` pentru detalii.
