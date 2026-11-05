statuslist = [[0.8, 1, 1, 1, 1, 1, 0.8],
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

status_exclusivities = [[2],[],[0,6],[],[8],[15],[2],[],[4],[13],[],[14],[12],[],[11],[],[],[],[19],[18],[12]]
# exapmles of thingies that might be used, pay attention to length of the lists
#N_effect = 2
#C_effects = [True, False, False, False,False, False, False, False, False, False,False,False,False,False,False,False,False,False,False,False,False,False,]
#stats= [1,1,1,1,1,1,1]
def status_change(stats, C_effects, N_effect):
    for i in status_exclusivities[N_effect]:
        if C_effects[i] == True:
            wascancelled = True
            C_effects[i]=False
            for k in range(0,len(stats)):
                    stats[k]= stats[k]/statuslist[i][k]
    if wascancelled == True:
        return stats, C_effects
    for i in range(0,len(stats)):
        stats[i]= stats[i]*statuslist[N_effect][i]
    C_effects[0] = True
    return stats, C_effects


