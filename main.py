
import arcade
import constant

from constant import (
    SCREEN_WIDTH,
    SCREEN_HEIGHT,
    SCREEN_TITLE,
    BACKGROUND_PATH,
    BACKGROUND_COLOUR,
    TEXT_COLOUR,
    HIGHLIGHT_COLOUR,
    BUTTON_COLOUR,
)


class MainMenuView(arcade.View):
    def __init__(self):
        super().__init__()

        self.background = None

        try:
            self.background = arcade.load_texture(BACKGROUND_PATH)
        except (FileNotFoundError, OSError):
            print("Background image not found. Using a plain background.")

        self.buttons = [
            ("PLAY / NEW GAME", 400),
            ("SETTINGS", 320),
            ("CREDITS", 240),
            ("QUIT", 160),
        ]

        self.mouse_x = -1
        self.mouse_y = -1

    def on_draw(self):
        self.clear(BACKGROUND_COLOUR)

        # Draw the background image
        if self.background:
            arcade.draw_texture_rect(
                self.background,
                arcade.XYWH(
                    SCREEN_WIDTH / 2,
                    SCREEN_HEIGHT / 2,
                    SCREEN_WIDTH,
                    SCREEN_HEIGHT,
                ),
            )

        # Dark panel behind the menu
        arcade.draw_rect_filled(
            arcade.XYWH(320, 360, 440, 610),
            (0, 0, 0, 190),
        )

        # Game title
        arcade.draw_text(
            "Not Ready Yet",
            320,
            570,
            TEXT_COLOUR,
            38,
            anchor_x="center",
            font_name="serif",
            bold=True,
        )

        arcade.draw_text(
            "Idk Leif, Isobel and Ethan made",
            320,
            520,
            HIGHLIGHT_COLOUR,
            21,
            anchor_x="center",
            font_name="serif",
        )

        # Draw each menu button
        for label, y in self.buttons:
            hovered = (
                100 <= self.mouse_x <= 540
                and y - 25 <= self.mouse_y <= y + 25
            )

            colour = HIGHLIGHT_COLOUR if hovered else BUTTON_COLOUR

            arcade.draw_rect_filled(
                arcade.XYWH(320, y, 440, 52),
                colour,
            )

            arcade.draw_rect_outline(
                arcade.XYWH(320, y, 440, 52),
                TEXT_COLOUR,
                border_width=1,
            )

            arcade.draw_text(
                label,
                320,
                y,
                TEXT_COLOUR,
                18,
                anchor_x="center",
                anchor_y="center",
                font_name="serif",
            )

        arcade.draw_text(
            "Come play plssss",
            320,
            85,
            (145, 145, 145),
            12,
            anchor_x="center",
        )

    def on_mouse_motion(self, x, y, dx, dy):
        self.mouse_x = x
        self.mouse_y = y

    def on_mouse_press(self, x, y, button, modifiers):
        if button != arcade.MOUSE_BUTTON_LEFT:
            return

        for label, button_y in self.buttons:
            inside = (
                100 <= x <= 540
                and button_y - 25 <= y <= button_y + 25
            )

            if not inside:
                continue

            if label == "Play":
                print("Starting a new game...")

            elif label == "Settings":
                print("Settings selected.")

            elif label == "Cretits":
                print("Credits selected.")

            elif label == "QUIT":
                arcade.exit()

            break


def main():
    window = arcade.Window(
        SCREEN_WIDTH,
        SCREEN_HEIGHT,
        SCREEN_TITLE,
    )

    menu = MainMenuView()
    window.show_view(menu)

    arcade.run()


if __name__ == "__main__":
    main()
