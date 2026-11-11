import arcade
import math


from constant import SCREEN_WIDTH, SCREEN_HEIGHT

SPEED = 5
SCREEN_TITLE = "Circle of Squares in Python Arcade"


class CircleOfSquaresView(arcade.Window):

    def __init__(self):
        super().__init__(SCREEN_WIDTH, SCREEN_HEIGHT, SCREEN_TITLE, resizable=True)
        arcade.set_background_color(arcade.color.WHITE)
        self.camera = None
        
        # Simple player setup
        self.player = arcade.SpriteCircle(radius=15, color=arcade.color.LA_SALLE_GREEN)
        self.player.center_x = 400
        self.player.center_y = 300
        
        # Create a SpriteList container for your player sprite
        self.player_list = arcade.SpriteList()
        self.player_list.append(self.player)
        
        # OPTIMISATION: Variable to hold our batched shapes on the GPU
        self.grid_shape_list = None
        
        # Track keyboard keys
        self.left_pressed = False
        self.right_pressed = False
        self.up_pressed = False
        self.down_pressed = False

    def setup(self):
        self.camera = arcade.Camera2D()
        
        self.grid_shape_list = arcade.shape_list.ShapeElementList()
        smaller_grid = 40       
        square_size = 1000
        circle_radius = square_size * 50
        center_x = 400
        center_y = 300
        grid_radius = int(circle_radius / square_size)
        grid_radius_small = int(circle_radius / square_size)
        for column in range(-grid_radius_small, grid_radius_small + 1):
            for row in range(-grid_radius_small, grid_radius_small + 1):
                offset_x = column * smaller_grid
                offset_y = row * smaller_grid
                if offset_x**2 + offset_y**2 > (circle_radius*smaller_grid)**2:
                    continue

                rect_shape = arcade.shape_list.create_rectangle_outline(
                    center_x=center_x + offset_x,
                    center_y=center_y + offset_y,
                    width=square_size,
                    height=square_size,
                    color=arcade.color.ORANGE,
                    border_width=2
                )
                self.grid_shape_list.append(rect_shape)
        for column in range(-grid_radius, grid_radius + 1):
            for row in range(-grid_radius, grid_radius + 1):
                offset_x = column * square_size
                offset_y = row * square_size
                if offset_x**2 + offset_y**2 > circle_radius**2:
                    continue
                rect_shape = arcade.shape_list.create_rectangle_outline(
                    center_x=center_x + offset_x,
                    center_y=center_y + offset_y,
                    width=square_size,
                    height=square_size,
                    color=arcade.color.BLUE,
                    border_width=2
                )
                self.grid_shape_list.append(rect_shape)



    def on_update(self, delta_time):
        if self.right_pressed: self.player.center_x += SPEED
        if self.left_pressed: self.player.center_x -= SPEED
        if self.up_pressed: self.player.center_y += SPEED
        if self.down_pressed: self.player.center_y -= SPEED

        target_x = self.player.center_x
        target_y = self.player.center_y
        
        self.camera.position = arcade.math.lerp_2d(self.camera.position, (target_x, target_y), 0.1)

    def on_draw(self):
        self.clear()
        
        self.camera.use()

        self.grid_shape_list.draw()

        self.player_list.draw()

    def on_key_press(self, key, modifiers):
        if key == arcade.key.UP: self.up_pressed = True
        elif key == arcade.key.DOWN: self.down_pressed = True
        elif key == arcade.key.LEFT: self.left_pressed = True
        elif key == arcade.key.RIGHT: self.right_pressed = True

    def on_key_release(self, key, modifiers):
        if key == arcade.key.UP: self.up_pressed = False
        elif key == arcade.key.DOWN: self.down_pressed = False
        elif key == arcade.key.LEFT: self.left_pressed = False
        elif key == arcade.key.RIGHT: self.right_pressed = False


def main():
    hi = CircleOfSquaresView()
    hi.setup()
    arcade.run()


if __name__ == "__main__":
    main()
