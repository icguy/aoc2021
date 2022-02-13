import numpy as np

filename = "./problems/p19.txt"

class Scanner():
	def __init__(self, coords):
		self.coords = coords

	def genHashes(self):
		size = len(self.coords)
		triplets = []
		for i in range(size - 2):
			for j in range(i + 1, size - 1):
				for k in range(j + 1, size):
					triplets.append((self.coords[i], self.coords[j], self.coords[k]))
		self.hashes = [self.genHash(t) for t in triplets]
	
	def genHash(self, triplet):
		a = triplet[0] * 3
		b = triplet[1] * 3
		c = triplet[2] * 3

		center = np.floor_divide(a + b + c, 3)

		ar = a - center
		br = b - center
		cr = c - center

		hashes = []
		for v in [ar, br, cr]:
			arr = sorted([abs(x) for x in [v[0], v[1], v[2]]])
			hashes.append(str(arr))
		return str(sorted(hashes))

f = open(filename)
input = f.read()
lines = input.splitlines()
scanners = []
scanner = None
for line in lines:
	if len(line.strip()) == 0:
		continue
	if line.startswith('---'):
		if scanner:
			scanners.append(scanner)
			scanner = None
	else:
		if not scanner:
			scanner = Scanner([])
		probe = np.array([int(a) for a in line.split(',')], dtype=np.int32)
		scanner.coords.append(probe)

scanners.append(scanner)
for s in scanners:
	s.genHashes()

print("hashing done")

# scanner = scanners[0]
# other_scanners = [s for s in scanners if s != scanner]
# counts = [0, 0, 0]
# for hash in scanner.hashes:
# 	containing = len([os for os in other_scanners if hash in os.hashes])
# 	if containing == 0:
# 		counts[0] += 1
# 	elif containing == 1:
# 		counts[1] += 1
# 	else:
# 		counts[2] += 1
# print(counts)

for scanner in scanners:
	other_scanners = [s for s in scanners if s != scanner]
	nums = []
	for os in other_scanners:
		num_matching = len([h for h in os.hashes if h in scanner.hashes])
		if num_matching >= 220:
			nums.append((num_matching, scanners.index(os)))

	print(scanners.index(scanner), nums)

	
