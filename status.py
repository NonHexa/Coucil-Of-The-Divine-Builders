status = [[0.8, 1, 1, 1, 1, 1, 0.8],
          [0.7, 1, 1, 1, 1, 0.8, 1],
          [1, 1, 1, 0.8, 1, 1, 1],
          [0, 0, 0, 0, 0, 0, 0],
          [1.3 ,1 ,1 ,0.5 ,1 ,0.5 ,0.5],
          [0.5 ,1 ,1 ,1 ,1 ,0.5 ,1.15],
          [0.7 ,1 ,1 ,1 ,1 ,0.7 ,0.8],
          [1, 1, 1, 1, 1, 1, 0.8],
          [0.7 ,1 ,1 ,1 ,1 ,1 ,2],
          [0.5, 1.4, 1.4, 0.6, 0.5, 1, 1.2],
          [0.6, 1.1, 1.1, 0.6, 0.6, 0.8, 0.9],
          [0.9, 1, 1, 1, 1, 0.9, 1],
          [0.8, 1.5, 1.2, 1, 1, 1, 1],
          [1, 1, 1, 0.9, 0.9, 1, 1],
          [1, 1, 1, 1, 1, 1, 0.8],
          [0.8, 1, 1, 1, 1, 0.75, 0.7],
          [0.8, 1, 1, 0.8, 0.8, 0.85, 0.7],
          [1, 1, 1, 1, 1, 1, 0],
          [1, 1, 1, 1, 1, 1, 1],
          [1, 1.2, 1.2, 1, 1, 1, 1],
          [1, 0.5, 0.6, 1, 1, 1, 1]]

def wet(stats, C_effects):
    if C_effects[2] == True:
        return stats, C_effects
    for i in range(0,len(stats)):
        stats[i]= stats[i]*status[0][i]
    C_effects[0] = True
    return stats, C_effects
    


status = [10,10,10,10,10,10,10]
ailments = [False,False,False,False,False,False,False]

(status, ailments) = wet(status, ailments)

print(status, ailments)