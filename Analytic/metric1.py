import json
import matplotlib.pyplot as plt
import numpy as np

# Load JSON data
with open("cs526-senior-associates-default-rtdb-export.json") as f:
    data = json.load(f)

# Initialize dictionaries to store aggregated counts by level
level_counts = {}

for key, value in data['data'].items():
    level = value['level']
    if level not in level_counts:
        level_counts[level] = {'deliveredNum': 0, 'failedNum': 0, 'schmoozeNum': 0}
    level_counts[level]['deliveredNum'] += value.get('deliveredNum', 0)
    level_counts[level]['failedNum'] += value.get('failedNum', 0)
    level_counts[level]['schmoozeNum'] += value.get('schmoozeNum', 0)

# Prepare data for plotting
levels = sorted(level_counts.keys())
deliveredNum_counts = [level_counts[level]['deliveredNum'] for level in levels]
failedNum_counts = [level_counts[level]['failedNum'] for level in levels]
schmoozeNum_counts = [level_counts[level]['schmoozeNum'] for level in levels]

# Plot the data
x = np.arange(len(levels))
width = 0.25

fig, ax = plt.subplots(figsize=(12, 8))
rects1 = ax.bar(x - width, deliveredNum_counts, width, label='deliveredNum')
rects2 = ax.bar(x, failedNum_counts, width, label='failedNum')
rects3 = ax.bar(x + width, schmoozeNum_counts, width, label='schmoozeNum')

# Add some text for labels, title and custom x-axis tick labels, etc.
ax.set_xlabel('Level')
ax.set_ylabel('Counts')
ax.set_title('Counts of deliveredNum, failedNum, and schmoozeNum by level')
ax.set_xticks(x)
ax.set_xticklabels(levels)
ax.legend()

# Function to autolabel bars
def autolabel(rects):
    """Attach a text label above each bar in *rects*, displaying its height."""
    for rect in rects:
        height = rect.get_height()
        ax.annotate('{}'.format(height),
                    xy=(rect.get_x() + rect.get_width() / 2, height),
                    xytext=(0, 3),  # 3 points vertical offset
                    textcoords="offset points",
                    ha='center', va='bottom')

autolabel(rects1)
autolabel(rects2)
autolabel(rects3)

fig.tight_layout()
plt.show()