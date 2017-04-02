using UnityEngine;

/// <summary>
/// Physics2Dの拡張クラス
/// </summary>
public static class Physics2DExtentsion {

	//Rayの表示時間
	private const float RAY_DISPLAY_TIME = 3;

	/// <summary>
	/// Rayを飛ばすと同時に画面に線を描画する
	/// </summary>
	public static RaycastHit2D RaycastAndDraw(Vector2 origin, Vector2 direction, float maxDistance, int layerMask){
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

		//衝突時のRayを画面に表示
		if (hit.collider){
			Debug.DrawRay(origin, hit.point - origin, Color.blue, RAY_DISPLAY_TIME, false);
		}
		//非衝突時のRayを画面に表示
		else{
			Debug.DrawRay(origin, direction * maxDistance, Color.green, RAY_DISPLAY_TIME, false);
		}

		return hit;
	}

}