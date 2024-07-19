from matplotlib.colors import Normalize
import matplotlib.pyplot as plt
from PIL import Image, ImageDraw
import numpy as np
import json

length = [24, 24, 36]
LEVEL = 2
ORIGIN = [(729, 340), (757, 350), (757, 350)]
PIXEL_PER_GAME_UNIT = [71.1538, 71.1538, 73.0769]


def find_last_index(lst, value):
    try:
        return len(lst) - 1 - lst[::-1].index(value)
    except ValueError:
        return -1


def convert_game_coord_to_pixel_pos(game_coord):
    x = ORIGIN[LEVEL][0] + game_coord[0] * PIXEL_PER_GAME_UNIT[LEVEL]
    y = ORIGIN[LEVEL][1] - game_coord[1] * PIXEL_PER_GAME_UNIT[LEVEL]
    return [int(round(x)), int(round(y))]


player_pos = [[], [], []]
kpi_lengths = [[], [], []]
with open("cs526-senior-associates-default-rtdb-export.json", encoding="utf-8") as f:
    data = json.load(f)
    data = data["data"]
    for _, d in data.items():
        level = int(d["level"]) - 1
        trend = d["kpiTrend"]
        first_valid_index = find_last_index(trend, 0)
        trend = trend[first_valid_index:]
        x_list = d["playerX"][first_valid_index:]
        y_list = d["playerY"][first_valid_index:]
        if level == 0:
            while trend[0] < 6:
                trend = trend[1:]
                x_list = x_list[1:]
                y_list = y_list[1:]
        x_list, y_list = x_list[:length[level]], y_list[:length[level]]
        while len(x_list) < length[level]:
            x_list.append(x_list[-1])
            y_list.append(y_list[-1])

        for x, y in zip(x_list, y_list):
            player_pos[level].append(convert_game_coord_to_pixel_pos([x, y]))


def create_heatmap(image_path, coordinates, circle_radius):
    # Load the image
    img = Image.open(image_path)
    width, height = img.size

    # Create an empty image for the heatmap
    heatmap = np.zeros((height, width))

    # Overlay circles onto the heatmap
    for coord in coordinates:
        x, y = coord
        y_indices, x_indices = np.ogrid[:height, :width]
        mask = (x_indices - x)**2 + (y_indices - y)**2 <= circle_radius**2
        heatmap[mask] += 1

    # Normalize heatmap values to [0, 1]
    norm = Normalize(vmin=heatmap.min(), vmax=heatmap.max())
    heatmap_normalized = norm(heatmap)

    # Create a colormap
    colormap = plt.colormaps.get_cmap('jet')
    heatmap_colored = colormap(heatmap_normalized)

    # Convert the colormap to an image
    heatmap_img = Image.fromarray((heatmap_colored[:, :, :3] * 255).astype(np.uint8))

    # Blend the heatmap with the original image
    blended_img = Image.blend(img.convert('RGB'), heatmap_img, alpha=0.5)

    # Save the output image with a modified filename
    output_path = f"{image_path.rsplit('.', 1)[0]}_heatmap.png"
    blended_img.save(output_path)
    print(f"Heatmap saved to {output_path}")


create_heatmap(f'level{LEVEL + 1}.png', player_pos[LEVEL], PIXEL_PER_GAME_UNIT[LEVEL] * 0.26)
