import tkinter as tk

login_screen_x = 350
login_screen_y = 250

login_screen = tk.Tk()
login_screen.title("title")
login_screen.geometry(f"{login_screen_x}x{login_screen_y}")
login_screen.configure(bg="")

username = ...
password = ...

login_screen.mainloop()