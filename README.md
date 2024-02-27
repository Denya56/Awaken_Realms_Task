# Awaken_Realms_Task 
Dice throwing
My solution to the test task from Awaken Realms interview.

Skończony projekt powinien pozwalać użytkownikowi rzucać dwunastościenną kostką.
Kostka powinna zachowywać się możliwie najbardziej jak prawdziwa, czyli toczyć
się zgodnie z prawami fizyki, a rejestrowane wyniki muszą odpowiadać temu, co
faktycznie wypadło na trójwymiarowej kostce.

Dokładniejsza specyfikacja projektu:
  1. do każdej ścianki kostki powinna być przypisana liczba, widoczna dla użytkownika konkretne liczby przypisane do poszczególnych ścianek kostki muszą być w łatwy sposób edytowalne z poziomu inspektora
  2. kostka powinna obsługiwać drag’n’drop, a właściwie drag’n’roll:
     - po przytrzymaniu lewego przycisku myszki użytkownik “podnosi” kostkę
     - po puszczeniu przycisku kostka zostaje rzucona, zachowując prędkość nadaną myszką
     - jeśli kostka ma zbyt mało prędkości żeby rzut był odpowiednio “losowy”, kostka powinna po prostu wrócić na powierzchnię stołu bez wykonywania rzutu
  3. z prawej strony znajduje się prosty interfejs użytkownika, udostępniający trzy rzeczy:
     - pole tekstowe opisane “Result:”, pokazujące:
       - “?” kiedy kostka jest w ruchu
       - wynik ostatniego rzutu kiedy już jest znany
       - drugie pole tekstowe opisane “Total:”, pokazujące sumę wyników wyrzuconych do tej pory
       - przycisk “Roll” automatycznie rzucający kostką (bez potrzeby korzystania z wyżej wspomnianego drag’n’dropa)
  4. kostka nie może nigdy opuścić “stołu”, niezależnie od akcji użytkownika
  5. każdy rzut musi kończyć się uzyskaniem wyniku
