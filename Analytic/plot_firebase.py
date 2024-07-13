import json
import matplotlib.pyplot as plt
import numpy as np

wrong_item_list = np.zeros((3,2))


# Step 1: Load the JSON file
with open('cs526-senior-associates-default-rtdb-export.json', 'r') as f:
    data_origin = json.load(f)

data_whole = data_origin['data']

for data_index in data_whole:
    data = data_whole[data_index]
    wrong_item = data['wrongItemNum']
    level = int( data['level'] )
    wrong_item_list[level-1,0] += wrong_item
    wrong_item_list[level-1, 1] += 1
    asdf = 1


labels = ['Level 1', 'Level 2', 'Level 3']
values = [wrong_item_list[0,0]/wrong_item_list[0,1], wrong_item_list[1,0]/wrong_item_list[1,1], wrong_item_list[2,0]/wrong_item_list[2,1]]

# Create a bar chart
plt.figure(figsize=(8, 6))  # Set the figure size


# Plot the bars with skyblue color
plt.bar(labels, values, color='skyblue')

# Set labels and title
plt.title('Incorrect item delivery attempt rate')
# plt.xlabel('Categories')  # X-axis label
# plt.ylabel('Values')      # Y-axis label


plt.grid(True)  # Show grid lines
plt.tight_layout()  # Adjust layout to prevent clipping of labels

plt.show()  # Display the chart