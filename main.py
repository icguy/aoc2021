import numpy as np

global scanners

filename = "./problems/p19ex.txt"

class Scanner():
	def __init__(self, coords):
		self.coords = coords

	def genHashes(self):
		size = len(self.coords)
		self.hashes = []
		for i in range(size - 1):
			for j in range(i + 1, size):
				self.hashes.append((self.genHash(self.coords[i], self.coords[j]), i, j))
	
	def genHash(self, a, b):
		v = a - b
		return str(sorted([abs(x) for x in [v[0], v[1], v[2]]]))

def genRot(a, b):
	aa, bb = np.abs(a), np.abs(b)
	rot = np.zeros((3, 3), np.int32)
	for i in range(3):
		for j in range(3):
			if(aa[i] == bb[j]):
				rot[i, j] = 1
	brot = np.matmul(a, rot)

	sign = np.eye(3, dtype=np.int32)
	for i in range(3):
		if b[i] != brot[i]:
			sign[i, i] = -1
	
	rot = np.matmul(rot, sign)
	# np.matmul(a, rot) == b
	return rot

# returns rot, d such that: s2 * rot + d = s1
def getTransformInner(s1, s2, idxp, rot):
	s2rot = np.matmul(s2.coords, rot)
	idx1, idx2 = idxp
	dv = s1.coords[idx1] - s2rot[idx2]
	d = np.matmul(np.ones((s2rot.shape[0], 1), dtype=np.int32), dv.reshape((1, 3)))
	s2rot_trans = s2rot + d
	a = s2rot_trans
	b = np.array(s1.coords)
	count = 0
	for i in range(a.shape[0]):
		av = a[i,:]
		for j in range(b.shape[0]):
			bv = b[j,:]
			if(np.array_equal(av, bv)):
				count += 1
	return (rot, dv) if count >= 12 else None

def getTransform(pair):
	global scanners

	s1 = scanners[pair[0]]
	s2 = scanners[pair[1]]
	matches = pair[2]
	hash2 = matches[0]
	hash1 = next(h for h in s1.hashes if h[0] == hash2[0])
	rot1 = genRot(s2.coords[hash2[1]] - s2.coords[hash2[2]], s1.coords[hash1[1]] - s1.coords[hash1[2]])
	rot2 = rot1 * -1
	idxp1 = (hash1[1], hash2[1])
	idxp2 = (hash1[1], hash2[2])
	ts = [
		getTransformInner(s1, s2, idxp1, rot1),
		getTransformInner(s1, s2, idxp2, rot1),
		getTransformInner(s1, s2, idxp1, rot2),
		getTransformInner(s1, s2, idxp2, rot2)
	]
	for t in ts:
		if(t):
			return t

def transformPoints(coords, t):
	rot, dv = t
	d = np.matmul(np.ones((coords.shape[0], 1), dtype=np.int32), dv.reshape((1, 3)))
	s2rot = np.matmul(coords, rot)
	return s2rot + d

class TreeNode:
	def __init__(self, idx, pair):
		self.idx = idx
		self.children = []
		self.coords = None
		self.pair = pair

def buildTree(pairs):
	used_indexes = []
	remaining = [p for p in pairs]
	tree = TreeNode(0, None)
	queue = [tree]
	while len(queue) > 0:
		curr = queue.pop()
		used_indexes.append(curr.idx)
		for r in remaining:
			idx_add = None
			if r[0] == curr.idx:
				idx_add = r[1]
			if idx_add != None:
				new_node = TreeNode(idx_add, r)
				curr.children.append(new_node)
				queue.append(new_node)
		remaining = [r for r in remaining if r[0] != curr.idx and r[1] != curr.idx]
	return tree

def getAllCoords(tree):
	global scanners

	allcoords = []
	for child in tree.children:
		t = getTransform(child.pair)
		coords = getAllCoords(child)
		points = transformPoints(coords, t)
		allcoords.append(points)
		pass
	allcoords.append(scanners[tree.idx].coords)
	return np.concatenate(allcoords, axis=0)

def run():
	global scanners

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

	# pairs = [(sidx, oidx, [o_hash, ...]), ...]
	# o_hash = (hash, i, j)
	pairs = []

	hashes = [[h[0] for h in s.hashes] for s in scanners]

	for scanner in scanners:
		sidx = scanners.index(scanner)
		for osidx in range(len(scanners)):
			if osidx == sidx:
				continue

			matching = []
			for oshidx in range(len(hashes[osidx])):
				if hashes[osidx][oshidx] in hashes[sidx]:
					matching.append(scanners[osidx].hashes[oshidx])
			if len(matching) >= 66:
				print(sidx, osidx)
				pair = (sidx, osidx, matching)
				pairs.append(pair)
	
	tree = buildTree(pairs)
	coords = getAllCoords(tree)
	print(np.unique(coords, axis=0).shape)

	print("done")

run()